using ProductApi.Entities;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProductApi
{
    public class ProductDbContext : DbContext
    {
        public IConfiguration Configuration { get; }
        public ProductDbContext(DbContextOptions<ProductDbContext> options, IConfiguration configuration) : base(options) 
        { 
            Configuration = configuration;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserProduct> UsersProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //configure primary key for UserProduct
            modelBuilder.Entity<UserProduct>()
                .HasKey(up => new
                { 
                    up.UserId, 
                    up.ProductId 
                });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
