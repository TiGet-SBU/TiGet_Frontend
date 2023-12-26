using Domain.Entities;
using Domain.Enums;
using Domain.Structs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions.DataSeed
{
    public static class DataSeeder
    {
        public static void SeedValuesInDateBase(this ModelBuilder modelBuilder)
        {
            modelBuilder.SeedAdmins();
            modelBuilder.SeedVehicles();
            //modelBuilder.SeedStations();
        }

        private static void SeedAdmins(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                    new Customer
                    {
                        Id = Guid.NewGuid(),
                        Email = "admin@admin.com",

                        // todo: make better?!
                        PasswordHash = "$2a$11$.64fLerPDfuVgkHnbF3o6uBF1MGQqfxYoPivqq8HkwvevmKIbT5gy", // 1234
                        Role = Role.Admin,
                        CreatedDate = DateTime.Now,
                    }
                );
        }

        private static void SeedVehicles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasData(
                    new Vehicle
                    {
                        Id = Guid.NewGuid(),
                        Name = "Bus1",
                        Capacity = 30,
                        Type = VehicleType.Bus,
                        CreatedDate = DateTime.Now,
                    },
                    new Vehicle
                    {
                        Id = Guid.NewGuid(),
                        Name = "Bus2",
                        Capacity = 40,
                        Type = VehicleType.Bus,
                        CreatedDate = DateTime.Now,
                    },
                    new Vehicle
                    {
                        Id = Guid.NewGuid(),
                        Name = "Bus3",
                        Capacity = 20,
                        Type = VehicleType.Bus,
                        CreatedDate = DateTime.Now,
                    },
                    new Vehicle
                    {
                        Id = Guid.NewGuid(),
                        Name = "Train1",
                        Capacity = 70,
                        Type = VehicleType.Train,
                        CreatedDate = DateTime.Now,
                    },
                    new Vehicle
                    {
                        Id = Guid.NewGuid(),
                        Name = "Train2",
                        Capacity = 88,
                        Type = VehicleType.Train,
                        CreatedDate = DateTime.Now,
                    },
                    new Vehicle
                    {
                        Id = Guid.NewGuid(),
                        Name = "Airplane1",
                        Capacity = 50,
                        Type = VehicleType.Airplane,
                        CreatedDate = DateTime.Now,
                    }
                );
        }

        private static void SeedStations(this ModelBuilder modelBuilder)
        {
            Guid Id = Guid.NewGuid();
            modelBuilder.Entity<Station>().HasData(
               
                    new Station
                    {
                        Name = "Test station",
                        Id = Guid.NewGuid(),
                        CityId = Id,
                        City = new City {
                            Id = Id,
                            CityName = "Tehran",
                            Province = "Tehran",
                            CreatedDate = DateTime.Now
                        },
                        //Location = new Location { x = 123, y = 132},
                        Address = "some address",
                        vehicleType = VehicleType.Bus,
                        CreatedDate = DateTime.Now,
                       
                    }

                );
        }
    }
}
