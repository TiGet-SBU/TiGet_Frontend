using Microsoft.AspNetCore.Identity;

using System;
using System.Threading.Tasks;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System.Globalization;
using Rhazes.Services.Identity.Infrastructure.Repositories;

namespace Rhazes.Services.Identity.API.Infrastructure.Services
{
    public class CustomPhoneNumberTokenProvider : IUserTwoFactorTokenProvider<ApplicationUser>
    {
        public CustomPhoneNumberTokenProvider() :base()
        {
        }
       

        public async Task<string> GenerateAsync(string purpose, UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            var token = await manager.CreateSecurityTokenAsync(user);
            return CustomRfc6238AuthenticationService.GenerateCode(token).ToString("D6", CultureInfo.InvariantCulture);
        }

        public async Task<bool> ValidateAsync(string purpose, string token, UserManager<ApplicationUser> manager, ApplicationUser user)
        {
           
            var securityToken = await manager.CreateSecurityTokenAsync(user);
            var result = CustomRfc6238AuthenticationService.ValidateCode(securityToken, token);
            return result;
        }

        public async Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            var phoneNumber = await manager.GetPhoneNumberAsync(user);
            return !string.IsNullOrWhiteSpace(phoneNumber) && await manager.IsPhoneNumberConfirmedAsync(user);
        }
    }
}
