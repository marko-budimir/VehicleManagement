namespace MVC.Models
{
    public class VehicleModelViewModel
    {
        public VehicleModelDto[] VehicleModels { get; set; }
        public VehicleMakeDto[] VehicleMakes { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
