using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Shopping.DAL;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Item)
            .WithMany(i => i.OrderDetails)
            .HasForeignKey(od => od.ItemId);

        modelBuilder.Entity<Item>()
            .HasOne(i => i.UnitOfMeasure)
            .WithMany(u => u.Items)
            .HasForeignKey(i => i.UomId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        //==
        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>();

        modelBuilder.Entity<User>()
            .Property(o => o.Role)
            .HasConversion<string>();
        //==
        modelBuilder.Entity<Item>(entity =>
        {
            entity.Property(e => e.Quantity).HasDefaultValue(0);
            entity.Property(e => e.Price).HasDefaultValue(decimal.Zero);
        });

        modelBuilder.Seed();
    }

}
// LazyLoading
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ShoppingDB;Integrated Security=True;Trust Server Certificate=True");

        return new AppDbContext(optionsBuilder.Options);
    }
}
