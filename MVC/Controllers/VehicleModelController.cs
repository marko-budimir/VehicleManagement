using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Utilities;
using Service;
using Service.Enums;
using Service.Models;
using System.Data;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class VehicleModelController : Controller
    {
        private const string ActionNameModel = ControllerConstants.ActionNameModel;
        private const string ActionNameEdit = ControllerConstants.ActionNameEditModel;
        private const string ErrorMessageData = ControllerConstants.ErrorMessageData;
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

        public async Task<IActionResult> Model(string searchString, Guid? selectedMake = null, VehicleSortOrder sortOrder = VehicleSortOrder.NameAsc, int pageNumber = 1)
        {
            ViewData[ControllerConstants.NameSortData] = sortOrder == VehicleSortOrder.NameAsc ? VehicleSortOrder.NameDesc : VehicleSortOrder.NameAsc;
            ViewData[ControllerConstants.AbrvSortData] = sortOrder == VehicleSortOrder.AbrvAsc ? VehicleSortOrder.AbrvDesc : VehicleSortOrder.AbrvAsc;
            ViewData[ControllerConstants.MakeNameSortData] = sortOrder == VehicleSortOrder.MakeNameAsc ? VehicleSortOrder.MakeNameDesc : VehicleSortOrder.MakeNameAsc;
            ViewData[ControllerConstants.SortData] = sortOrder;
            ViewData[ControllerConstants.CurrentFilterData] = searchString;
            ViewData[ControllerConstants.SelectedMakeData] = selectedMake;

            var pagedVehicleModels = await _vehicleModelService.GetAllAsync(sortOrder, pageNumber, searchString, selectedMake);
            var pagedVehicleMakes = await _vehicleMakeService.GetAllAsync(VehicleSortOrder.NameAsc);
            var model = _mapper.Map<VehicleModelViewModel>(pagedVehicleModels);
            model.VehicleMakes = _mapper.Map<VehicleMakeDto[]>(pagedVehicleMakes.Items);
            return View(model);
        }

        [Authorize(Roles = UserConstants.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVehicleModel(VehicleModelDto newModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(ActionNameModel);
            }
            VehicleModel model = _mapper.Map<VehicleModel>(newModel);
            var successful = false;
            try
            {
                successful = await _vehicleModelService.AddAsync(model);
            }
            catch (DuplicateNameException e)
            {
                TempData[ErrorMessageData] = e.Message;
                return RedirectToAction(ActionNameModel);
            }
            catch (Exception e)
            {
                TempData[ErrorMessageData] = e.Message;
                return RedirectToAction(ActionNameModel);
            }
            if (!successful)
            {
                return BadRequest(new { error = "Could not add model." });
            }
            return RedirectToAction(ActionNameModel);
        }

        [Authorize(Roles = UserConstants.AdminRole)]
        public async Task<IActionResult> DeleteVehicleModel(Guid id)
        {
            var successful = await _vehicleModelService.DeleteAsync(id);
            if (!successful)
            {
                return BadRequest(new { error = "Could not delete model." });
            }
            return RedirectToAction(ActionNameModel);
        }

        [Authorize(Roles = UserConstants.AdminRole)]
        public async Task<IActionResult> EditVehicleModel(Guid id)
        {
            var model = await _vehicleModelService.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            var pagedVehicleMakes = await _vehicleMakeService.GetAllAsync(VehicleSortOrder.NameAsc);
            VehicleModelDto modelDto = _mapper.Map<VehicleModelDto>(model);
            modelDto.vehicleMakes = _mapper.Map<VehicleMakeDto[]>(pagedVehicleMakes.Items);
            return View(modelDto);
        }

        [Authorize(Roles = UserConstants.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVehicleModel(VehicleModelDto modelDto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(ActionNameEdit, new { id = modelDto.Id });
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
                TempData[ErrorMessageData] = e.Message;
                return RedirectToAction(ActionNameEdit, new { id = modelDto.Id });
            }
            if (!successful)
            {
                return BadRequest(new { error = "Could not update model." });
            }
            return RedirectToAction(ActionNameModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
