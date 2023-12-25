using System.Threading.Tasks;
using Rhazes.BuildingBlocks.EventBus.Abstractions;
using Serilog.Context;
using Rhazes.Services.Identity.API.IntegrationEvents.Events;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.Commands;
using Rhazes.Services.Identity.API.Application.IntegrationEvents.Events;

namespace Rhazes.Services.Identity.API.Application.IntegrationEvents.EventHandling
{
    public class AddUserPatientIntegrationEventHandler : IIntegrationEventHandler<AddUserPatientIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly IIdentityIntegrationEventService _identityIntegrationEventService;

        public AddUserPatientIntegrationEventHandler(
            IMediator mediator,
            IIdentityIntegrationEventService identityIntegrationEventService
            )
        {
            _mediator = MethodParameterChecker.CheckUp(mediator);
            _identityIntegrationEventService = MethodParameterChecker.CheckUp(identityIntegrationEventService);
        }

        public async Task Handle(AddUserPatientIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {

                if (@event.PhoneNumber != null)
                {
                    @event.PhoneNumber = @event.PhoneNumber.StartsWith("09") || @event.PhoneNumber.StartsWith("9") ? @event.PhoneNumber : null;
                }

                var createUserPatientCommand = new CreateOrUpdateUserPatientCommand(
                    @event.NationalCode,
                    @event.Name,
                    @event.LastName,
                    @event.PhoneNumber,
                    @event.Gender,
                    @event.Birthdate,
                     @event.CreateById,
                     @event.NationalityId,
                     null, null, null, null, null, null, null, null, null, null, null, null, null
                    );

                var result = await _mediator.Send(createUserPatientCommand);

                CreateOrUpdatePatientIntegrationEvent changeStaffPhoneNumberIntegrationEvent = new CreateOrUpdatePatientIntegrationEvent(
                      @event.PhoneNumber,
                      result.Name,
                      result.LastName,
                      result.NationalCode,
                      result.Gender,
                      result.Birthdate,
                      result.Id.Value,
                      @event.CreateById,
                      @event.NationalityId,
                      result.FatherName,
                      result.Email,
                      result.MaritalType,
                      result.BirthStateId,
                      result.BirthCityId,
                      result.Tel,
                      result.StateId,
                      result.CityId,
                      result.Address,
                      result.ZipCode,
                      result.BloodTypeId,
                      result.SpouseBloodTypeId,
                      result.EducationId,
                      result.JobId
                      );

                await _identityIntegrationEventService.AddAndSaveEventAsync(changeStaffPhoneNumberIntegrationEvent);
                await _identityIntegrationEventService.PublishThroughEventBusAsync(changeStaffPhoneNumberIntegrationEvent);
            }
        }
    }
}