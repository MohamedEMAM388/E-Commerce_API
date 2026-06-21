using Microsoft.EntityFrameworkCore;
using ECommerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order = ECommerce.Domain.Entities.OrderModule.Order;

namespace ECommerce.Persistence.Data.Configurations
{
    public class OrderConfiguratins : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.SubTotal)
                   .HasPrecision(8, 2);

            builder.OwnsOne(x => x.Address, OE =>
                {
                    OE.Property(x => x.FirstName).HasMaxLength(50);
                    OE.Property(x => x.LastName).HasMaxLength(50);
                    OE.Property(x => x.Street).HasMaxLength(100);
                    OE.Property(x => x.City).HasMaxLength(50);
                    OE.Property(x => x.Country).HasMaxLength(50);

                }
            );
                  

        }
    }
}
