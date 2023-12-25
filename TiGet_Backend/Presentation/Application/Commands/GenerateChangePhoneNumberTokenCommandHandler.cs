using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class GenerateChangePhoneNumberTokenCommandHandler : IRequestHandler<GenerateChangePhoneNumberTokenCommand, string>
    {
        private readonly IApplicationUserManager _userManager;

        public GenerateChangePhoneNumberTokenCommandHandler(
            IApplicationUserManager userManager
            )
        {
            _userManager = MethodParameterChecker.CheckUp(userManager);
        }

        public async Task<string> Handle(GenerateChangePhoneNumberTokenCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(command.UserId.ToString());
            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, command.PhoneNumber);

            return token;
        }
    }

    public class TwoFactorTokenDTO
    {
        public string Token { get; set; }
    }
}
