using ExcelsisDeo.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExcelsisDeo.Persistence.Configurations
{
    public class BasketConfiguration : IEntityTypeConfiguration<Basket>, IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.ToTable("Baskets");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);
        }

        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.ToTable("BasketItems");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();

            builder.HasOne(x => x.Basket).WithMany(z => z.BasketItems).HasForeignKey(x => x.BasketId);
        }
    }
}
