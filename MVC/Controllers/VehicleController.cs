using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Service;

namespace MVC.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        public async Task<IActionResult> Index()
        {
            var vehicleModel = await _vehicleService.GetVehicleModelsAsync();
            var model = new VehicleModelViewModel
            {
                VehicleModels = vehicleModel
            };
            return View(model);
        }
    }
}
