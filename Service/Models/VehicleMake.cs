using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Service.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class VehicleMake
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abrv { get; set; }
        public ICollection<VehicleModel> VehicleModels { get; set; } = new List<VehicleModel>();
    }
}
