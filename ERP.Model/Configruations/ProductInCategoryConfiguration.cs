using ERP.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Model.Configruations
{
    public class ProductInCategoryConfiguration : IEntityTypeConfiguration<ProductIncategory>
    {
        public void Configure(EntityTypeBuilder<ProductIncategory> builder)
        {
            // lien ket key
            builder.HasKey(t => new { t.CategoryId, t.ProductId });

            builder.ToTable("ProductInCategories");

            //ket noi tu bang carogory den product n-n

            builder.HasOne(t => t.Product).WithMany(pc => pc.ProductInCategories)
            .HasForeignKey(pc => pc.ProductId);

            //ket noi tu bang product den carogory n-n

            builder.HasOne(t => t.Category).WithMany(pc => pc.ProductInCategories)
            .HasForeignKey(pc => pc.CategoryId);
        }
    }
}
