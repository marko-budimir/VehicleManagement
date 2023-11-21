using Microsoft.EntityFrameworkCore;
using Service.Data;
using Service.Enums;
using Service.Models;
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
            _dbContext.VehicleModels.Remove(new VehicleModel { Id = id });

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

        public async Task<VehicleModel[]> GetAllAsync(VehicleSortOrder sortOrder = VehicleSortOrder.NameAsc)
        {
            var query = _dbContext.VehicleModels.AsQueryable();

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
                    query = query.OrderBy(m => m.Abrv);
                    break;
            }
            return await query.ToArrayAsync();
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
