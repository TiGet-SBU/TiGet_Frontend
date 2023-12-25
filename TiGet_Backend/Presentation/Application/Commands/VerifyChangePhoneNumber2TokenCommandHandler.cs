using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.BuildingBlocks.Common.Exception;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class VerifyChangePhoneNumber2TokenCommandHandler : IRequestHandler<VerifyChangePhoneNumber2TokenCommand, bool>
    {

        private readonly IApplicationUserManager _userManager;

        public VerifyChangePhoneNumber2TokenCommandHandler(
           IApplicationUserManager applicationUserManager
            )
        {
            _userManager = MethodParameterChecker.CheckUp(applicationUserManager);
        }

        public async Task<bool> Handle(VerifyChangePhoneNumber2TokenCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(command.UserId.ToString());

            var result = await _userManager.ChangePhoneNumber2Async(user, command.PhoneNumber, command.Token);
            if (!result.Succeeded)
            {
                throw new DomainException("TOKEN_IS_INVALID");
            }
            return true;
        }
    }
}
