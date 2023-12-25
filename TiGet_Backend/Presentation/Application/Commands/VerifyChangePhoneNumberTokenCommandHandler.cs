using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.BuildingBlocks.Common.Exception;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class VerifyChangePhoneNumberTokenCommandHandler : IRequestHandler<VerifyChangePhoneNumberTokenCommand, bool>
    {
        private readonly IApplicationUserManager _userManager;

        public VerifyChangePhoneNumberTokenCommandHandler(
            IApplicationUserManager applicationUserManager
            )
        {
            _userManager = MethodParameterChecker.CheckUp(applicationUserManager);
        }

        public async Task<bool> Handle(VerifyChangePhoneNumberTokenCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(command.UserId.ToString());

            var result = await _userManager.ChangePhoneNumberAsync(user, command.PhoneNumber, command.Token);
            if (!result.Succeeded)
            {
                throw new DomainException("TOKEN_IS_INVALID");
            }

            return true;
        }
    }
}
