using Autofac;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Rhazes.BuildingBlocks.Common.Infrastructure;
using Rhazes.Services.Identity.API.Infrastructure.Services;
using Rhazes.Services.Identity.Domain.AggregatesModel.RoleAggregate;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.Services.Identity.Domain.Seedwork;
using Rhazes.Services.Identity.Domain.Validators;
using Rhazes.Services.Identity.Infrastructure.Data;
using Rhazes.Services.Identity.Infrastructure.Repositories;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace Rhazes.Services.Identity.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {

        public string QueriesConnectionString { get; }

        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;

        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterType<UpperInvariantLookupNormalizer>().As<ILookupNormalizer>().InstancePerLifetimeScope();
            builder.RegisterType<SecurityStampValidator<ApplicationUser>>().As<ISecurityStampValidator>().InstancePerLifetimeScope();
            builder.RegisterType<PasswordValidator<ApplicationUser>>().As<IPasswordValidator<ApplicationUser>>().InstancePerLifetimeScope();
            builder.RegisterType<UserValidator<ApplicationUser>>().As<IUserValidator<ApplicationUser>>().InstancePerLifetimeScope();

            builder.RegisterType<UserClaimsPrincipalFactory<ApplicationUser>>().As<IUserClaimsPrincipalFactory<ApplicationUser>>().InstancePerLifetimeScope();
            builder.RegisterType<UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>>().As<UserClaimsPrincipalFactory<ApplicationUser>>().InstancePerLifetimeScope();
             
            builder.RegisterType<ApplicationUserStore>().As<IApplicationUserStore>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationRoleStore>().As<IApplicationRoleStore>().InstancePerLifetimeScope(); 

            builder.RegisterType<PermissionService>().As<IPermissionService>();

            builder.RegisterType<IdentityErrorDescriber>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationUserManager>().As<IApplicationUserManager>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationSignInManager>().As<IApplicationSignInManager>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationRoleManager>().As<IApplicationRoleManager>().InstancePerLifetimeScope();
            builder.RegisterType<TokenProviderDescriptor>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultTokenService>().As<ITokenService>().InstancePerLifetimeScope();

            builder.RegisterType<IhioService>().As<IIhioService>().InstancePerLifetimeScope();
            builder.RegisterType<TenantService>().As<ITenantService>().InstancePerLifetimeScope();
            builder.RegisterType<HumanService>().As<IHumanService>().InstancePerLifetimeScope();
            builder.RegisterType<IrimcService>().As<IIrimcService>().InstancePerLifetimeScope();

            builder.RegisterType<HttpClient>().InstancePerLifetimeScope();

            builder.RegisterType<PasswordHasher<ApplicationUser>>().As<IPasswordHasher<ApplicationUser>>().InstancePerLifetimeScope();



            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly(), typeof(UserRepository).Assembly)
             .Where(t => t.IsClosedTypeOf(typeof(IRepository<>))).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly(), typeof(IMapper<,>).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IMapper<,>))).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IValidator<>).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>))).AsImplementedInterfaces().InstancePerLifetimeScope();

        }
    }
}
