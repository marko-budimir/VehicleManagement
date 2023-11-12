using Service.Models;

namespace Service
{
    public class FakeVehicleService : IVehicleService
    {
        private readonly VehicleMake[] _vehicleMakes = new VehicleMake[]
        {
            new VehicleMake { Name = "Ford Motor Company", Abrv = "Ford" },
            new VehicleMake { Name = "General Motors", Abrv = "GM" },
            new VehicleMake { Name = "Toyota Motor Corporation", Abrv = "Toyota" },
            new VehicleMake { Name = "Honda Motor Company", Abrv = "Honda" },
            new VehicleMake { Name = "Audio AG", Abrv = "Audi" },
            new VehicleMake { Name = "Bayerische Motoren Werke AG", Abrv = "BMW" },
            new VehicleMake { Name = "Volkswagen", Abrv = "VW" },
        };
        public Task<VehicleMake[]> GetVehicleMakesAsync()
        {
            return Task.FromResult(_vehicleMakes);
        }

        public Task<VehicleModel[]> GetVehicleModelsAsync()
        {
            return Task.FromResult(new VehicleModel[]
            {
                new VehicleModel { Name = "Audi A5", Abrv = "A5", VehicleMake = _vehicleMakes[4]},
                new VehicleModel { Name = "Audi A6", Abrv = "A6", VehicleMake = _vehicleMakes[4]},
                new VehicleModel { Name = "Audi A7", Abrv = "A7", VehicleMake = _vehicleMakes[4]},
                new VehicleModel { Name = "VW Golf 7", Abrv = "Golf 7", VehicleMake = _vehicleMakes[6]},
                new VehicleModel { Name = "VW Golf 8", Abrv = "Golf 8", VehicleMake = _vehicleMakes[6]},
                new VehicleModel { Name = "VW Passat", Abrv = "Passat", VehicleMake = _vehicleMakes[6]},
                new VehicleModel { Name = "BMW G20", Abrv = "BMW 3 Series", VehicleMake = _vehicleMakes[5]},
                new VehicleModel { Name = "BMW G30", Abrv = "BMW 5 Series", VehicleMake = _vehicleMakes[5]},
            });
        }
    }
}
