using Microsoft.EntityFrameworkCore;

namespace RedisSampleProject.API.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
               new Product() { Id = 1, Name = "pencil", Price = 25 },
               new Product() { Id=2,Name="pen",Price=15},
               new Product() { Id=3,Name="book",Price=5}
               
               );


            base.OnModelCreating(modelBuilder);
        }
    }
}
