using Service.Models;

namespace Service
{
    public interface IVehicleMakeService
    {
        Task<VehicleMake[]> GetAllAsync();
        Task<bool> AddAsync(VehicleMake newMake);
        Task<VehicleMake?> GetByIdAsync(Guid? id);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateAsync(VehicleMake make);
    }
}
