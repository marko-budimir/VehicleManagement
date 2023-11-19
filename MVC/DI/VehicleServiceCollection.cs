using Service;

namespace MVC.DI
{
    public static class VehicleServiceCollection
    {
        public static IServiceCollection AddVehicleModule(this IServiceCollection services)
        {
            services.AddScoped<IVehicleModelService, VehicleModelService>();
            services.AddScoped<IVehicleMakeService, VehicleMakeService>();
            return services;
        }
    }
}
