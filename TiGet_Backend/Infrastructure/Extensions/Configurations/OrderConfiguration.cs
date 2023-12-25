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
    public class OrderConfiguration : BaseEntityConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(e => e.SitPos)
                .IsRequired();
            builder.Property(e => e.CustomerId)
                .IsRequired();
            builder.Property(e => e.TicketId)
                .IsRequired();
            builder.Property(e => e.TicketOwnerFirstName)
                .IsRequired();
            builder.Property(e => e.TicketOwnerLastName)
                .IsRequired();
            builder.Property(e => e.NationalId)
                .IsRequired();

            builder.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Ticket)
                .WithMany()
                .HasForeignKey(e => e.TicketId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
