namespace Rhazes.Services.Identity.API.Infrastructure.Services
{
    public interface IRedirectService
    {
        string ExtractRedirectUriFromReturnUrl(string url);
    }
}
