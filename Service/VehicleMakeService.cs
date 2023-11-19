using Microsoft.EntityFrameworkCore;
using Service.Data;
using Service.Models;
using System.Data;

namespace Service
{
    public class VehicleMakeService : IVehicleMakeService
    {
        private readonly ApplicationDbContext _dbContext;

        public VehicleMakeService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddAsync(VehicleMake newMake)
        {
            newMake.Id = Guid.NewGuid();
            _dbContext.VehicleMakes.Add(newMake);
            try
            {
                var result = await _dbContext.SaveChangesAsync();
                return result == 1;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException is not null && e.InnerException.Message.Contains("IX_VehicleMakes_Name"))
                    throw new DuplicateNameException("Vehicle make name already exists.");
                else
                    throw new Exception("An error occurred while saving the vehicle make.");
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var make = await _dbContext.VehicleMakes.Include(vm => vm.VehicleModels).FirstOrDefaultAsync(m => m.Id == id);

            if (make == null)
            {
                return false;
            }


            foreach (var model in make.VehicleModels)
            {
                model.VehicleMake = null;
            }

            _dbContext.VehicleMakes.Remove(make);

            try
            {
                var result = await _dbContext.SaveChangesAsync();
                return result >= 1;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<VehicleMake?> GetByIdAsync(Guid? id)
        {
            return await _dbContext.VehicleMakes.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<VehicleMake[]> GetAllAsync()
        {
            return await _dbContext.VehicleMakes.ToArrayAsync();
        }

        public async Task<bool> UpdateAsync(VehicleMake make)
        {
            _dbContext.VehicleMakes.Update(make);
            try
            {
                var result = await _dbContext.SaveChangesAsync();
                return result == 1;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException is not null && e.InnerException.Message.Contains("IX_VehicleMakes_Name"))
                    throw new DuplicateNameException("Vehicle make name already exists.");
                else
                    throw new Exception("An error occurred while updating the vehicle make.");
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
