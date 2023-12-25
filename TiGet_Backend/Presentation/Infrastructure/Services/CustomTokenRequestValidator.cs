using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.Domain.Validators
{
    public class CustomTokenRequestValidator : ICustomTokenRequestValidator
    {
        public Task ValidateAsync(CustomTokenRequestValidationContext context)
        {
            context.Result.ValidatedRequest.Client.AccessTokenLifetime = 10;

            return Task.FromResult(0);
        }
    }
}
