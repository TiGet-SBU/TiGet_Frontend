using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Infrastructure.Devspaces
{
    static class IdentityDevspacesBuilderExtensions
    {
        public static IIdentityServerBuilder AddDevspacesIfNeeded(this IIdentityServerBuilder builder, bool useDevspaces)
        {
            if (useDevspaces)
            {
                builder.AddRedirectUriValidator<DevspacesRedirectUriValidator>();
            }
            return builder;
        }
    }
}
