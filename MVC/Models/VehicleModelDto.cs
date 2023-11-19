using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class VehicleModelDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abrv { get; set; }
        public Guid? VehicleMakeId { get; set; }
        public VehicleMakeDto? VehicleMake { get; set; }
        public VehicleMakeDto[]? vehicleMakes { get; set; }

        public VehicleModelDto()
        {
            vehicleMakes = null;
        }

        public VehicleModelDto(VehicleMakeDto[] vehicleMakes)
        {
            this.vehicleMakes = vehicleMakes;
        }
    }
}
