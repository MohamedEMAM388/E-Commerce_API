using ECommerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.Configurations
{
    internal class OrderItemconfigurations : IEntityTypeConfiguration<ItemsOrder>
    {
        public void Configure(EntityTypeBuilder<ItemsOrder> builder)
        {
            builder.Property(oi => oi.price)
                   .HasPrecision(8, 2);
            builder.OwnsOne(oi => oi.product, p => {

                p.Property(x => x.ProductName)
                 .HasMaxLength(100);
                p.Property(x => x.PictureUrl)
                 .HasMaxLength(200);

            });
        }
    }
}
