using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.DTO;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Rhazes.BuildingBlocks.Common.Exception;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    public class VerifyTwoFactorTokenCommandHandler : IRequestHandler<VerifyTwoFactorTokenCommand, bool>
    {
        private readonly IApplicationUserManager _userManager;

        public VerifyTwoFactorTokenCommandHandler(
            IApplicationUserManager userManager
            )
        {
            _userManager = MethodParameterChecker.CheckUp(userManager);
        }

        public async Task<bool> Handle(VerifyTwoFactorTokenCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(command.UserId.ToString());

            var verified = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, command.Token);
            if (verified)
                return verified;
            else
                throw new DomainException("TOKEN_IS_INVALID");
        }
    }
}
