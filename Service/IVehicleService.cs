using Service.Models;

namespace Service
{
    public interface IVehicleService
    {
        Task<VehicleMake[]> GetVehicleMakesAsync();
        Task<VehicleModel[]> GetVehicleModelsAsync();
        Task<bool> AddVehicleModelAsync(VehicleModel newModel);
        Task<bool> AddVehicleMakeAsync(VehicleMake newMake);
        Task<VehicleMake?> GetVehicleMakeByIdAsync(Guid? id);
    }
}
