using Microsoft.EntityFrameworkCore;
using Service.Data;
using Service.Models;

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
            var result = await _dbContext.SaveChangesAsync();
            return result == 1;
        }

        public async Task<bool> AddVehicleModelAsync(VehicleModel newModel)
        {
            newModel.Id = Guid.NewGuid();
            _dbContext.VehicleModels.Add(newModel);
            var result =  await _dbContext.SaveChangesAsync();
            return result == 1;
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
