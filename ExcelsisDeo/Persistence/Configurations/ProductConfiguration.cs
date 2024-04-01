using ExcelsisDeo.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExcelsisDeo.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description);
            builder.Property(x => x.Price).HasPrecision(19, 4);
            builder.Property(x => x.InStockQuantity);

            builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
        }
    }
}
