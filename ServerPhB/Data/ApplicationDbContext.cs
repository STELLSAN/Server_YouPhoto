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
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Salon> Salons { get; set; }
    }
}
