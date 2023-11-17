using Service.Models;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class VehicleModelDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abrv { get; set; }
        public Guid? VehicleMakeId { get; set; }
        public readonly VehicleMake[]? _vehicleMakes;

        public VehicleModelDto()
        {
            _vehicleMakes = null;
        }

        public VehicleModelDto(VehicleMake[] vehicleMakes)
        {
            _vehicleMakes = vehicleMakes;
        }
    }
}
