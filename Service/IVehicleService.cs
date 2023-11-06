using Service.Models;

namespace Service
{
    public interface IVehicleService
    {
        Task<VehicleMake[]> GetVehicleMakesAsync();
        Task<VehicleModel[]> GetVehicleModelsAsync();
    }
}
