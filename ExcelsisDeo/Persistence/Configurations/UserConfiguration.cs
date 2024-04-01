using ExcelsisDeo.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExcelsisDeo.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.FullName).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Role).IsRequired();
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.LastUpdatedAt).ValueGeneratedOnUpdate();
            //builder.Property(x => x.RefreshToken).IsRequired(false);
        }
    }
}