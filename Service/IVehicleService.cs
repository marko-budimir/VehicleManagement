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
        Task<VehicleModel?> GetVehicleModelByIdAsync(Guid? id);
        Task<bool> DeleteVehicleModelAsync(Guid id);
        Task<bool> DeleteVehicleMakeAsync(Guid id);
        Task<bool> UpdateVehicleMakeAsync(VehicleMake make);
        Task<bool> UpdateVehicleModelAsync(VehicleModel model);
    }
}
