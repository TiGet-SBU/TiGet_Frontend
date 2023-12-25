using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rhazes.BuildingBlocks.EventBus.Abstractions;
using Rhazes.BuildingBlocks.EventBus.Events;
using Rhazes.BuildingBlocks.IntegrationEventLogEF;
using Rhazes.BuildingBlocks.IntegrationEventLogEF.Services;
using Rhazes.BuildingBlocks.IntegrationEventLogEF.Utilities;
using Rhazes.Services.Identity.Infrastructure.Data;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents
{
    public class IdentityIntegrationEventService : IIdentityIntegrationEventService, IDisposable
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly ApplicationDbContext _identityContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<IdentityIntegrationEventService> _logger;
        private volatile bool disposedValue;

        public IdentityIntegrationEventService(
            IEventBus eventBus,
            ApplicationDbContext identityContext,
            IntegrationEventLogContext eventLogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
            ILogger<IdentityIntegrationEventService> logger
            )
        {
            _identityContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_identityContext.Database.GetDbConnection());
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

            foreach (var logEvt in pendingLogEvents)
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", logEvt.EventId, Program.AppName, logEvt.IntegrationEvent);

                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    _eventBus.Publish(logEvt.IntegrationEvent);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", logEvt.EventId, Program.AppName);

                    await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);

            try
            {
                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                _eventBus.Publish(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", evt.Id, Program.AppName);

                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);
            try
            {
                await _eventLogService.SaveEventAsync(evt, _identityContext.GetCurrentTransaction());
            }
            catch (Exception ex)
            {
                await ResilientTransaction.New(_identityContext).ExecuteAsync(async () =>
                {
                    // Achieving atomicity between original catalog database operation and the IntegrationEventLog thanks to a local transaction
                    await _identityContext.SaveChangesAsync();
                    await _eventLogService.SaveEventAsync(evt, _identityContext.Database.CurrentTransaction);
                });
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    (_eventLogService as IDisposable)?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
