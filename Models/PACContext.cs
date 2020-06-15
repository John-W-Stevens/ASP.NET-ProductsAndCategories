using System;
using Microsoft.EntityFrameworkCore;

namespace ProductsAndCategories.Models
{
    public class PACContext : DbContext
    {
        public PACContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Association> Associations { get; set; }
    }
}

