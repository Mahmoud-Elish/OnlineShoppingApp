using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.DAL;

public static class SeedData
{
    public static readonly List<UnitOfMeasure> UnitOfMeasures = new()
    {
        new UnitOfMeasure { Id = 1, UOM = "PCS", Description = "Pieces" },
        new UnitOfMeasure { Id = 2, UOM = "KG", Description = "Kilogram" },
        new UnitOfMeasure { Id = 3, UOM = "L", Description = "Liter" }
    };

    public static readonly List<Item> Items = new()
    {
        new Item
        {
            Id = 1,
            ItemName = "Laptop",
            Description = "High-performance laptop",
            UomId = 1,
            Quantity = 50,
            Price = 999.99m
        },
        new Item
        {
            Id = 2,
            ItemName = "Coffee",
            Description = "Premium coffee beans",
            UomId = 2,
            Quantity = 100,
            Price = 29.99m
        }
    };

    public static readonly List<User> Users = new();
    public static readonly List<Order> Orders = new();
    public static readonly List<OrderDetail> OrderDetails = new();

    static SeedData()
    {
        var hasher = new PasswordHasher<User>();
        var user1 = new User
        {
            Id = "1",
            Email = "admin@cs.cs",
            UserName = "admin",
            FullName = "admin",
            Role = Role.Admin
        };
        var user2 = new User
        {
            Id = "2",
            UserName = "john.doe@example.com",
            NormalizedUserName = "JOHN.DOE@EXAMPLE.COM",
            Email = "john.doe@example.com",
            NormalizedEmail = "JOHN.DOE@EXAMPLE.COM",
            EmailConfirmed = true,
            FullName = "John Doe",
            Role = Role.Customer
        };
        user1.PasswordHash = hasher.HashPassword(user1, "12345");
        user2.PasswordHash = hasher.HashPassword(user2, "Password123!");
        Users.Add(user1);

        
        Orders.Add(new Order
        {
            Id = 1,
            RequestDate = DateTime.UtcNow,
            Status = OrderStatus.Open,
            CustomerId = "1",
            CurrencyCode = "USD",
            ExchangeRate = 1,
            TotalPrice = 1029.98m,
            ForeignPrice = 1029.98m
        });

        
        OrderDetails.Add(new OrderDetail
        {
            Id = 1,
            OrderId = 1,
            ItemId = 1,
            ItemPrice = 999.99m,
            Quantity = 1,
            TotalPrice = 999.99m
        });
    }
}
public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UnitOfMeasure>().HasData(SeedData.UnitOfMeasures);
        modelBuilder.Entity<Item>().HasData(SeedData.Items);
        modelBuilder.Entity<User>().HasData(SeedData.Users);
        modelBuilder.Entity<Order>().HasData(SeedData.Orders);
        modelBuilder.Entity<OrderDetail>().HasData(SeedData.OrderDetails);
    }
}