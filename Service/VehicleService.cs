using Microsoft.EntityFrameworkCore;
using Service.Data;
using Service.Models;
using System.Data;

namespace Service
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _dbContext;

        public VehicleService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddVehicleMakeAsync(VehicleMake newMake)
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

        public async Task<bool> AddVehicleModelAsync(VehicleModel newModel)
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

        public async Task<bool> DeleteVehicleMakeAsync(Guid id)
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
                Console.WriteLine("DeleteVehicleMAkeAsync");
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteVehicleModelAsync(Guid id)
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

        public async Task<VehicleMake?> GetVehicleMakeByIdAsync(Guid? id)
        {
            return await _dbContext.VehicleMakes.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<VehicleMake[]> GetVehicleMakesAsync()
        {
            return await _dbContext.VehicleMakes.ToArrayAsync();
        }

        public async Task<VehicleModel[]> GetVehicleModelsAsync()
        {
           return await _dbContext.VehicleModels.ToArrayAsync();
        }
    }
}
