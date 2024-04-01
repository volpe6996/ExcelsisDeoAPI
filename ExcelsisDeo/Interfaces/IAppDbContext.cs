using ExcelsisDeo.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExcelsisDeo.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderElement> OrderElements { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
