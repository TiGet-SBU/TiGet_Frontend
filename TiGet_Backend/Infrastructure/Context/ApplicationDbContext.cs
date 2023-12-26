using Domain.Entities;
using Infrastructure.Extensions.Configurations;
using Infrastructure.Extensions.DataSeed;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions.Configurations;

namespace Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            // TODO: fix this part: 
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=TiGet_1;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true;";
            optionsBuilder.UseSqlServer(connectionString);
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CityConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new StationConfiguration());
            modelBuilder.ApplyConfiguration(new TicketConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleConfiguration());
            
            modelBuilder.SeedValuesInDateBase();
        }
        

        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<Station> Stations { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Other configurations...

            modelBuilder.ApplyConfiguration(new CustomerConfiguration()); // Add this line
        }
    }
}
