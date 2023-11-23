using Microsoft.EntityFrameworkCore;
using Service.Data;
using Service.Enums;
using Service.Models;
using Service.Utilities;
using System.Data;

namespace Service
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly ApplicationDbContext _dbContext;

        public VehicleModelService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddAsync(VehicleModel newModel)
        {
            newModel.Id = Guid.NewGuid();
            _dbContext.VehicleModels.Add(newModel);
            try
            {
                var result = await _dbContext.SaveChangesAsync();
                return result == 1;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException is not null && e.InnerException.Message.Contains("IX_VehicleModels_Name"))
                    throw new DuplicateNameException("Vehicle model name already exists.");
                else
                    throw new Exception("An error occurred while saving the vehicle model.");
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var model = await _dbContext.VehicleModels.FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return false;
            }

            _dbContext.VehicleModels.Remove(model);

            try
            {
                var result = await _dbContext.SaveChangesAsync();
                return result == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<VehicleModel?> GetByIdAsync(Guid? id)
        {
            return await _dbContext.VehicleModels.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<VehicleModel>> GetAllAsync(
            VehicleSortOrder sortOrder = VehicleSortOrder.NameAsc, 
            int? page = null, 
            string? searchString = null,
            Guid? selectedMake = null
            )
        {
            var query = _dbContext.VehicleModels.AsQueryable();
            if (selectedMake.HasValue)
                query = query.Where(m => m.VehicleMakeId == selectedMake);

            if (!String.IsNullOrEmpty(searchString))
                query = query.Where(m => m.Name.Contains(searchString) || m.Abrv.Contains(searchString));

            switch(sortOrder)
            {
                case VehicleSortOrder.MakeNameAsc:
                    query = query.OrderBy(m => m.VehicleMake == null ? "" : m.VehicleMake.Abrv);
                    break;
                case VehicleSortOrder.MakeNameDesc:
                    query = query.OrderByDescending(m => m.VehicleMake == null ? "" : m.VehicleMake.Abrv);
                    break;
                case VehicleSortOrder.AbrvAsc:
                    query = query.OrderBy(m => m.Abrv);
                    break;
                case VehicleSortOrder.AbrvDesc:
                    query = query.OrderByDescending(m => m.Abrv);
                    break;
                case VehicleSortOrder.NameDesc:
                    query = query.OrderByDescending(m => m.Name);
                    break;
                default:
                    query = query.OrderBy(m => m.Name);
                    break;
            }
            var count = await query.CountAsync();
            if (page.HasValue)
                query = query.Skip((page.Value - 1) * 10).Take(10);

            var result = await query.ToListAsync();

            return new PagedList<VehicleModel>(result, page ?? 1, 10, count);
        }

        public async Task<bool> UpdateAsync(VehicleModel model)
        {
            _dbContext.VehicleModels.Update(model);
            try
            {
                var result = await _dbContext.SaveChangesAsync();
                return result == 1;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException is not null && e.InnerException.Message.Contains("IX_VehicleModels_Name"))
                    throw new DuplicateNameException("Vehicle model name already exists.");
                else
                    throw new Exception("An error occurred while updating the vehicle model.");
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
