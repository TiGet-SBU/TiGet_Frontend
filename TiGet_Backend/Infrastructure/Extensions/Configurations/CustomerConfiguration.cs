using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions.Configurations;

namespace Infrastructure.Extensions.Configurations
{
    public class CustomerConfiguration : BaseEntityConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(e => e.Role)
                .IsRequired();

            builder.Property(e => e.Email)
                .IsRequired();

            builder.Property(e => e.PasswordHash)
                .IsRequired();

            // Ignore BirthDate property
            builder.Ignore(e => e.BirthDate);
        }
    }
}
