using Charlie.Order.DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Charlie.Order.DataAccess;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }
    public DbSet<OrderModel> Order { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<OrderModel>();

        base.OnModelCreating(modelBuilder);
    }
}