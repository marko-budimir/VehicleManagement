using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class VehicleModel
    {
        public Guid Id { get; set; }
        public Guid? MakeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abrv { get; set; }
        public VehicleMake? VehicleMake { get; set; }
    }
}
