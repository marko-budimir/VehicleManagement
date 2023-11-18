using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Service;
using Service.Models;
using System.Data;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public IActionResult Index()
        {
            
            return View();
        }
        public async Task<IActionResult> Model()
        {
            var vehicleModel = await _vehicleService.GetVehicleModelsAsync();
            var vehicleMake = await _vehicleService.GetVehicleMakesAsync();
            var model = new VehicleModelViewModel
            {
                VehicleModels = vehicleModel,
                VehicleMakes = vehicleMake
            };
            return View(model);
        }

        public async Task<IActionResult> Make()
        {
            var vehicleMake = await _vehicleService.GetVehicleMakesAsync();
            var model = new VehicleMakeViewModel
            {
                VehicleMakes = vehicleMake
            };
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVehicleModel(VehicleModelDto newModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Model");
            }
            VehicleMake? make = await _vehicleService.GetVehicleMakeByIdAsync(newModel.VehicleMakeId);
            VehicleModel model = new VehicleModel
            {
                Name = newModel.Name,
                Abrv = newModel.Abrv,
                VehicleMake = make
            };
            var successful = false;
            try {
                successful = await _vehicleService.AddVehicleModelAsync(model);
            }
            catch (DuplicateNameException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Model");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Model");
            }
            if (!successful)
            {
                return BadRequest(new { error = "Could not add model." });
            }
            return RedirectToAction("Model");
        }

        public async Task<IActionResult> DeleteVehicleModel(Guid id)
        {
            var successful = await _vehicleService.DeleteVehicleModelAsync(id);
            if (!successful)
            {
                return BadRequest(new { error = "Could not delete model." });
            }
            return RedirectToAction("Model");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVehicleMake(VehicleMakeDto newMake)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Make");
            }
            VehicleMake make = new VehicleMake
            {
                Name = newMake.Name,
                Abrv = newMake.Abrv
            };
            var successful = false;
            try
            {
                successful = await _vehicleService.AddVehicleMakeAsync(make);
            }
            catch (DuplicateNameException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Make");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Make");
            }
            if (!successful)
            {
                return BadRequest("Could not add make.");
            }
            return RedirectToAction("Make");
        }

        public async Task<IActionResult> DeleteVehicleMake(Guid id)
        {
            var successful = await _vehicleService.DeleteVehicleMakeAsync(id);
            if (!successful)
            {
                return BadRequest(new { error = "Could not delete make." });
            }
            return RedirectToAction("Make");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
