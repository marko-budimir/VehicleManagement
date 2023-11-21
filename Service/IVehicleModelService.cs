using Service.Enums;
using Service.Models;

namespace Service
{
    public interface IVehicleModelService
    {
        Task<VehicleModel[]> GetAllAsync(VehicleSortOrder sortOrder);
        Task<bool> AddAsync(VehicleModel newModel);
        Task<VehicleModel?> GetByIdAsync(Guid? id);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateAsync(VehicleModel model);
    }
}
