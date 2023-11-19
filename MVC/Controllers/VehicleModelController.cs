using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Service;
using Service.Models;
using System.Data;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class VehicleModelController : Controller
    {
        private readonly IVehicleModelService _vehicleModelService;
        private readonly IVehicleMakeService _vehicleMakeService;
        private readonly IMapper _mapper;

        public VehicleModelController(IVehicleModelService vehicleModelService, IVehicleMakeService vehicleMakeService, IMapper mapper)
        {
            _vehicleModelService = vehicleModelService;
            _vehicleMakeService = vehicleMakeService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> Model()
        {
            var vehicleModels = await _vehicleModelService.GetAllAsync();
            var vehicleMakes = await _vehicleMakeService.GetAllAsync();
            var model = new VehicleModelViewModel
            {
                VehicleModels = _mapper.Map<VehicleModelDto[]>(vehicleModels),
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
            try
            {
                successful = await _vehicleModelService.AddAsync(model);
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
            var successful = await _vehicleModelService.DeleteAsync(id);
            if (!successful)
            {
                return BadRequest(new { error = "Could not delete model." });
            }
            return RedirectToAction("Model");
        }

        public async Task<IActionResult> EditVehicleModel(Guid id)
        {
            var model = await _vehicleModelService.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            var vehicleMake = await _vehicleMakeService.GetAllAsync();
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
                vehicleModel.VehicleMake = await _vehicleMakeService.GetByIdAsync(modelDto.VehicleMakeId); ;
                successful = await _vehicleModelService.UpdateAsync(vehicleModel);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
