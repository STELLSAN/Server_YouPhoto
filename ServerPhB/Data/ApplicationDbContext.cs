using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ServerPhB.Models;

namespace ServerPhB.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial delivery methods
            modelBuilder.Entity<DeliveryMethod>().HasData(
                new DeliveryMethod
                {
                    DeliveryMethodID = 1,
                    Name = "Доставка",
                    Description = "Доставка фотографий курьером.",
                    Cost = 500
                },
                new DeliveryMethod
                {
                    DeliveryMethodID = 2,
                    Name = "Само-вывоз",
                    Description = "Само-вывоз фотографий из салона вручную.",
                    Cost = 0
                }
            );
        }
    }
}
