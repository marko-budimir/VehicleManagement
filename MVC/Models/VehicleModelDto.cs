using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class VehicleModelDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abrv { get; set; }
        public VehicleMakeDto? VehicleMake { get; set; }
    }
}
