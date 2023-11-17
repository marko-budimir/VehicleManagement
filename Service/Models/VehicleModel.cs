using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Service.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class VehicleModel
    {
        public Guid Id { get; set; }
        public Guid? VehicleMakeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abrv { get; set; }
        public VehicleMake? VehicleMake { get; set; }
    }
}
