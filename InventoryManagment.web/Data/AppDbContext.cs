using Microsoft.EntityFrameworkCore;
using InventoryManagment.web.Models;
namespace InventoryManagment.web.Data
{
    public class AppDbContext : DbContext
    { 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}
