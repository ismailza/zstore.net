using Microsoft.EntityFrameworkCore;
using zstore.net.Models;

namespace zstore.net.Data
{
  public class ZStoreDbContext : DbContext
  {
    public ZStoreDbContext(DbContextOptions<ZStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Admin> Admins { get; set; }
  }
}
