using Service.Enums;
using Service.Models;
using Service.Utilities;

namespace Service
{
    public interface IVehicleMakeService
    {
        Task<PagedList<VehicleMake>> GetAllAsync(VehicleSortOrder sortOrder, int? page = null);
        Task<bool> AddAsync(VehicleMake newMake);
        Task<VehicleMake?> GetByIdAsync(Guid? id);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateAsync(VehicleMake make);
    }
}
