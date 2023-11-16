using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class VehicleMakeDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abrv { get; set; }
    }
}
