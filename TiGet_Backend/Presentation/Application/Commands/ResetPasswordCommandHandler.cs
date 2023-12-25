using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.DTO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.BuildingBlocks.Common.Exception;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IApplicationUserManager _userManager;

        public ResetPasswordCommandHandler(
            IApplicationUserManager userManager
            )
        {

            _userManager = MethodParameterChecker.CheckUp(userManager);
        }

        public async Task<bool> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(command.UserId.ToString());
            var responseToken= await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, command.Token);
            if (!responseToken)
            {
                throw new DomainException("TOKEN_IS_INVALID");
            }
            var restToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, restToken, command.Password);
           
            return true;
        }
    }
}
