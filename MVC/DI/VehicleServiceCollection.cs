using Service;

namespace MVC.DI
{
    public static class VehicleServiceCollection
    {
        public static IServiceCollection AddVehicleModule(this IServiceCollection services)
        {
            services.AddScoped<IVehicleService, VehicleService>();
            return services;
        }
    }
}
