using ERP.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Model.Configruations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            // 1-n
            builder.ToTable("OrderDetails");

            builder.HasKey(x => new { x.ProductId, x.OrderId });

            builder.HasOne(x => x.Order).WithMany(x=>x.OrderDetails).HasForeignKey(x => x.OrderId);
            // 1-n
            builder.HasOne(x => x.Product).WithMany(x => x.OrderDetails).HasForeignKey(x => x.ProductId);

        }
    }
}
