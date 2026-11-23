using lab10_Jewellery_API.Models;
using Microsoft.EntityFrameworkCore;

namespace lab10_Jewellery_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<JewelleryItem> JewelleryItems { get; set; }
    }
}
