using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MySpot.Infrastructure.DAL
{
    internal static class Extention
    {
        public static IServiceCollection AddPostgres(this IServiceCollection services)
        {
            const string connectionString = "Host=localhost;Database=MySpot;Username=postgres;Password=";
            services.AddDbContext<MySpotDbContext>(x => x.UseNpgsql(connectionString));

            return services;
        }
    }
}
