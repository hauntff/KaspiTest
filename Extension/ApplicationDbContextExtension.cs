using KaspiTest.ApplicationContext;
using Microsoft.EntityFrameworkCore;

namespace KaspiTest.Extension
{
    public static class ApplicationDbContextExtension
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }
    }
}
