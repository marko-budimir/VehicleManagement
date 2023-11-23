using Microsoft.EntityFrameworkCore;
using Service.Data;
using Service.Enums;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Test
{
    public class VehicleModelServiceTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddVehicleModel()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AddAsync_ShouldAddVehicleModel")
                .Options;

            using var dbContext = new ApplicationDbContext(dbContextOptions);
            var vehicleModelService = new VehicleModelService(dbContext);

            // Act
            var result = await vehicleModelService.AddAsync(new VehicleModel { Name = "TestModel", Abrv = "TM" });

            // Assert
            Assert.True(result);

            var addedModel = dbContext.VehicleModels.FirstOrDefault(m => m.Name == "TestModel");
            Assert.NotNull(addedModel);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteVehicleModel()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAsync_ShouldDeleteVehicleModel")
                .Options;

            using var dbContext = new ApplicationDbContext(dbContextOptions);
            var vehicleModelService = new VehicleModelService(dbContext);

            // Add a vehicle model to the database for testing
            var modelToAdd = new VehicleModel { Name = "TestModel", Abrv = "TM" };
            await dbContext.VehicleModels.AddAsync(modelToAdd);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await vehicleModelService.DeleteAsync(modelToAdd.Id);

            // Assert
            Assert.True(result);

            var deletedModel = dbContext.VehicleModels.FirstOrDefault(m => m.Id == modelToAdd.Id);
            Assert.Null(deletedModel);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectVehicleModel()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetByIdAsync_ShouldReturnCorrectVehicleModel")
                .Options;

            using var dbContext = new ApplicationDbContext(dbContextOptions);
            var vehicleModelService = new VehicleModelService(dbContext);

            // Add a vehicle model to the database for testing
            var modelToAdd = new VehicleModel { Name = "TestModel", Abrv = "TM" };
            await dbContext.VehicleModels.AddAsync(modelToAdd);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await vehicleModelService.GetByIdAsync(modelToAdd.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(modelToAdd.Id, result?.Id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedList()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllAsync_ShouldReturnPagedList")
                .Options;

            using var dbContext = new ApplicationDbContext(dbContextOptions);
            var vehicleModelService = new VehicleModelService(dbContext);

            // Add multiple vehicle models to the database for testing
            var modelsToAdd = Enumerable.Range(1, 15)
                .Select(i => new VehicleModel { Name = $"TestModel{i}", Abrv = $"TM{i}" });

            await dbContext.VehicleModels.AddRangeAsync(modelsToAdd);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await vehicleModelService.GetAllAsync(sortOrder: VehicleSortOrder.NameAsc, page: 2, searchString: "TestModel");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(15, result.TotalCount);
            Assert.Equal(2, result.PageNumber);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateVehicleModel()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateAsync_ShouldUpdateVehicleModel")
                .Options;

            using var dbContext = new ApplicationDbContext(dbContextOptions);
            var vehicleModelService = new VehicleModelService(dbContext);

            // Add a vehicle model to the database for testing
            var modelToAdd = new VehicleModel { Name = "TestModel", Abrv = "TM" };
            await dbContext.VehicleModels.AddAsync(modelToAdd);
            await dbContext.SaveChangesAsync();
            
            // Act
            modelToAdd.Name = "UpdatedTestModel";
            var result = await vehicleModelService.UpdateAsync(modelToAdd);

            // Assert
            Assert.True(result);

            var updatedModel = dbContext.VehicleModels.FirstOrDefault(m => m.Id == modelToAdd.Id);
            Assert.NotNull(updatedModel);
            Assert.Equal("UpdatedTestModel", updatedModel.Name);
        }
    }
}
