using AutoMapper;
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
        private readonly IMapper _mapper;

        public VehicleController(IVehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            
            return View();
        }
        public async Task<IActionResult> Model()
        {
            var vehicleModels = await _vehicleService.GetVehicleModelsAsync();
            var vehicleMakes = await _vehicleService.GetVehicleMakesAsync();
            var model = new VehicleModelViewModel
            {
                VehicleModels = _mapper.Map<VehicleModelDto[]>(vehicleModels),
                VehicleMakes = _mapper.Map<VehicleMakeDto[]>(vehicleMakes)
            };
            return View(model);
        }

        public async Task<IActionResult> Make()
        {
            var vehicleMakes = await _vehicleService.GetVehicleMakesAsync();
            var model = new VehicleMakeViewModel
            {
                VehicleMakes = _mapper.Map<VehicleMakeDto[]>(vehicleMakes)
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
            VehicleModel model = _mapper.Map<VehicleModel>(newModel);
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

        public async Task<IActionResult> EditVehicleModel(Guid id)
        {
            var model = await _vehicleService.GetVehicleModelByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            Guid? VehicleMakeId = null;
            if (model.VehicleMake != null)
            {
                VehicleMakeId = model.VehicleMake.Id;
            }
            var vehicleMake = await _vehicleService.GetVehicleMakesAsync();
            VehicleModelDto modelDto = _mapper.Map<VehicleModelDto>(model);
            modelDto.vehicleMakes = _mapper.Map<VehicleMakeDto[]>(vehicleMake);
            return View(modelDto);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVehicleModel(VehicleModelDto modelDto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditVehicleModel", new { id = modelDto.Id });
            }
            var successful = false;
            try
            {
                var vehicleModel = _mapper.Map<VehicleModel>(modelDto);
                vehicleModel.VehicleMake = await _vehicleService.GetVehicleMakeByIdAsync(modelDto.VehicleMakeId); ;
                successful = await _vehicleService.UpdateVehicleModelAsync(vehicleModel);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("EditVehicleModel", new { id = modelDto.Id });
            }
            if (!successful)
            {
                return BadRequest(new { error = "Could not update model." });
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
            VehicleMake make = _mapper.Map<VehicleMake>(newMake);
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

        public async Task<IActionResult> EditVehicleMake(Guid id)
        {
            var make = await _vehicleService.GetVehicleMakeByIdAsync(id);
            if (make == null)
            {
                return NotFound();
            }
            var makeDto = _mapper.Map<VehicleMakeDto>(make);
            return View(makeDto);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVehicleMake(VehicleMakeDto makeDto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditVehicleMake", new { id = makeDto.Id });
            }
           
            var successful = false;
            try
            {
                var vehicleMake = _mapper.Map<VehicleMake>(makeDto);
                successful = await _vehicleService.UpdateVehicleMakeAsync(vehicleMake);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("EditVehicleMake", new { id = makeDto.Id });
            }
            if (!successful)
            {
                return BadRequest(new { error = "Could not update make." });
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
