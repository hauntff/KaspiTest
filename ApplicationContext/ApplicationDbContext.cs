using KaspiTest.Models;
using Microsoft.EntityFrameworkCore;

namespace KaspiTest.ApplicationContext
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public DbSet<News> News { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>().ToTable("News");
			modelBuilder.Entity<Person>().ToTable("Person");

		}
	}
}
