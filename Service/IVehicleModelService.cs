using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Enums;
using Service.Models;
using Service.Utilities;

namespace Service
{
    public interface IVehicleModelService
    {
        Task<PagedList<VehicleModel>> GetAllAsync(VehicleSortOrder sortOrder, int? page = null, string? searchString = null, Guid? selectedMake = null);
        Task<bool> AddAsync(VehicleModel newModel);
        Task<VehicleModel?> GetByIdAsync(Guid? id);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateAsync(VehicleModel model);
    }
}
