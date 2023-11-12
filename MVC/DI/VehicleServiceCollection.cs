using Service;

namespace MVC.DI
{
    public static class VehicleServiceCollection
    {
        public static IServiceCollection AddVehicleModule(this IServiceCollection services)
        {
            services.AddSingleton<IVehicleService, FakeVehicleService>();
            return services;
        }
    }
}
