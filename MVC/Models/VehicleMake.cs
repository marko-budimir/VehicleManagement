using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class VehicleMake
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abrv { get; set; }
        public ICollection<VehicleModel> VehicleModels { get; } = new List<VehicleModel>();
    }
}
