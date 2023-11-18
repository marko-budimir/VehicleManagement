using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class VehicleMakeDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abrv { get; set; }
    }
}
