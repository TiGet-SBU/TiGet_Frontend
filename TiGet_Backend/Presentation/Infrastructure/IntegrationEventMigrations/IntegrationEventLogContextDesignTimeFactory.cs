using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Rhazes.BuildingBlocks.IntegrationEventLogEF;
using System;

namespace Identity.API.Infrastructure.IntegrationEventMigrations
{
    public class IntegrationEventLogContextDesignTimeFactory : IDesignTimeDbContextFactory<IntegrationEventLogContext>
    {
        private readonly IConfiguration _configuration;
        public IntegrationEventLogContextDesignTimeFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IntegrationEventLogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IntegrationEventLogContext>();

            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStringIdentity") ?? _configuration["ConnectionString"], options => options.MigrationsAssembly(GetType().Assembly.GetName().Name));

            return new IntegrationEventLogContext(optionsBuilder.Options);
        }
    }
}