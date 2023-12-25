using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.IntegrationEvents;
using Rhazes.Services.Identity.API.Application.IntegrationEvents.Events;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class GenerateResetPasswordTokenCommandHandler : IRequestHandler<GenerateResetPasswordTokenCommand, string>
    {
        private readonly IApplicationUserManager _userManager;

        public GenerateResetPasswordTokenCommandHandler(
            IApplicationUserManager userManager
            )
        {
            _userManager = MethodParameterChecker.CheckUp(userManager);
        }

        public async Task<string> Handle(GenerateResetPasswordTokenCommand command, CancellationToken cancellationToken)
        {
            var token = await _userManager.GenerateTwoFactorTokenAsync(command.User, TokenOptions.DefaultPhoneProvider);
            return token;
        }
    }
}
