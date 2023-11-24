using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Services;

namespace MySpot.Application
{
    public static class Extentions
    {
        public static IServiceCollection AddAplication(this IServiceCollection services)
        {
            services.AddSingleton<IReservationService, ReservationService>();
            
            return services;
        }
    }
}
