using ExcelsisDeo.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExcelsisDeo.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>, IEntityTypeConfiguration<OrderElement>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Street).IsRequired();
            builder.Property(x => x.City).IsRequired();
            builder.Property(x => x.PostalCode).IsRequired();
        }

        public void Configure(EntityTypeBuilder<OrderElement> builder)
        {
            builder.ToTable("OrderElements");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();

            builder.HasOne(x => x.Order).WithMany(z => z.OrderElements).HasForeignKey(x => x.OrderId);
        }
    }
}
