using Microsoft.EntityFrameworkCore;
using Service.Data;
using Service.Enums;
using Service.Models;

namespace Service.Test
{
    public class VehicleMakeServiceTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddVehicleMake()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AddAsync_ShouldAddVehicleMake")
                .Options;

            using var dbContext = new ApplicationDbContext(dbContextOptions);
            var vehicleMakeService = new VehicleMakeService(dbContext);

            // Act
            var result = await vehicleMakeService.AddAsync(new VehicleMake { Name = "TestMake", Abrv = "TM" });

            // Assert
            Assert.True(result);

            // Check if the vehicle make was added to the database
            var addedMake = dbContext.VehicleMakes.FirstOrDefault(m => m.Name == "TestMake");
            Assert.NotNull(addedMake);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteVehicleMake()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAsync_ShouldDeleteVehicleMake")
                .Options;

            using var dbContext = new ApplicationDbContext(dbContextOptions);
            var vehicleMakeService = new VehicleMakeService(dbContext);

            // Add a vehicle make to the database for testing
            var makeToAdd = new VehicleMake { Name = "TestMake", Abrv = "TM" };
            await dbContext.VehicleMakes.AddAsync(makeToAdd);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await vehicleMakeService.DeleteAsync(makeToAdd.Id);

            // Assert
            Assert.True(result);

            var deletedMake = dbContext.VehicleMakes.FirstOrDefault(m => m.Id == makeToAdd.Id);
            Assert.Null(deletedMake);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectVehicleMake()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetByIdAsync_ShouldReturnCorrectVehicleMake")
                .Options;

            using var dbContext = new ApplicationDbContext(dbContextOptions);
            var vehicleMakeService = new VehicleMakeService(dbContext);

            // Add a vehicle make to the database for testing
            var makeToAdd = new VehicleMake { Name = "TestMake", Abrv = "TM" };
            await dbContext.VehicleMakes.AddAsync(makeToAdd);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await vehicleMakeService.GetByIdAsync(makeToAdd.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(makeToAdd.Id, result?.Id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedList()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllAsync_ShouldReturnPagedList")
                .Options;

            using var dbContext = new ApplicationDbContext(dbContextOptions);
            var vehicleMakeService = new VehicleMakeService(dbContext);

            // Add multiple vehicle makes to the database for testing
            var makesToAdd = Enumerable.Range(1, 15)
                .Select(i => new VehicleMake { Name = $"TestMake{i}", Abrv = $"TM{i}" });

            await dbContext.VehicleMakes.AddRangeAsync(makesToAdd);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await vehicleMakeService.GetAllAsync(sortOrder: VehicleSortOrder.NameAsc, page: 2, searchString: "TestMake");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(15, result.TotalCount);
            Assert.Equal(2, result.PageNumber);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateVehicleMake()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateAsync_ShouldUpdateVehicleMake")
                .Options;

            using var dbContext = new ApplicationDbContext(dbContextOptions);
            var vehicleMakeService = new VehicleMakeService(dbContext);

            // Add a vehicle make to the database for testing
            var makeToAdd = new VehicleMake { Name = "TestMake", Abrv = "TM" };
            await dbContext.VehicleMakes.AddAsync(makeToAdd);
            await dbContext.SaveChangesAsync();

            // Act
            makeToAdd.Name = "UpdatedTestMake";
            var result = await vehicleMakeService.UpdateAsync(makeToAdd);

            // Assert
            Assert.True(result);

            var updatedMake = dbContext.VehicleMakes.FirstOrDefault(m => m.Id == makeToAdd.Id);
            Assert.NotNull(updatedMake);
            Assert.Equal("UpdatedTestMake", updatedMake.Name);
        }
    }
}
