namespace Identity.API.Infrastructure
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Polly;
    using Polly.Retry;
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Identity.Infrastructure.Data;

    public class IdentityContextSeed
    {
        public async Task SeedAsync(ApplicationDbContext context, IWebHostEnvironment env, IOptions<IdentitySettings> settings, ILogger<IdentityContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(IdentityContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                var useCustomizationData = settings.Value.UseCustomizationData;

                var contentRootPath = env.ContentRootPath;

                using (context)
                {
                    context.Database.Migrate();

                    await context.SaveChangesAsync();
                }
            });
        }


        private AsyncRetryPolicy CreatePolicy(ILogger<IdentityContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );

        }
    }
}
