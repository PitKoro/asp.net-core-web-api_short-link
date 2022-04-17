using Microsoft.EntityFrameworkCore;
using ShortLinksApiApp.Data.EntityTypeConfiguration;
using ShortLinksApiApp.Data.Models;

namespace ShortLinksApiApp.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            /*Database.EnsureDeleted();
            Database.EnsureCreated();*/
        }
        public DbSet<ShortLink> ShortLinks => Set<ShortLink>();
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ShortLinkConfiguration());
            /*base.OnModelCreating(builder);*/
        }

    }
}
