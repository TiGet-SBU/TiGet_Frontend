using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions.Configurations
{
    public class StationConfiguration : BaseEntityConfiguration<Station>
    {
        public override void Configure(EntityTypeBuilder<Station> builder)
        {
            builder.Property(e => e.vehicleType)
                .IsRequired();

            builder.Property(e => e.CityId) 
                .IsRequired();

            builder.HasOne(e => e.City)
                .WithMany()
                .HasForeignKey(e => e.CityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
