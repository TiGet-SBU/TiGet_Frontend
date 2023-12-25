using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MediatR;
using Identity.API.Application.IntegrationEvents;
using Identity.API.Application.IntegrationEvents.Events;
using Identity.Domain.AggregatesModel.UserAggregate;

namespace Identity.API.Application.Commands
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
