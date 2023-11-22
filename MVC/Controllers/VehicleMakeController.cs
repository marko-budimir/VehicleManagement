using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Service;
using Service.Enums;
using Service.Models;
using System.Data;

namespace MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly IVehicleMakeService _vehicleMakeService;
        private readonly IMapper _mapper;

        public VehicleMakeController(IVehicleMakeService vehicleMakeService, IMapper mapper)
        {
            _vehicleMakeService = vehicleMakeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Make(string searchString,VehicleSortOrder sortOrder = VehicleSortOrder.NameAsc, int pageNumber = 1)
        {
            ViewData["NameSort"] = sortOrder == VehicleSortOrder.NameAsc ? VehicleSortOrder.NameDesc : VehicleSortOrder.NameAsc;
            ViewData["AbrvSort"] = sortOrder == VehicleSortOrder.AbrvAsc ? VehicleSortOrder.AbrvDesc : VehicleSortOrder.AbrvAsc;
            ViewData["Sort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;

            var paggedVehicleMakes = await _vehicleMakeService.GetAllAsync(sortOrder, pageNumber, searchString);
            var model = _mapper.Map<VehicleMakeViewModel>(paggedVehicleMakes);
            return View(model);
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
                successful = await _vehicleMakeService.AddAsync(make);
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
            var successful = await _vehicleMakeService.DeleteAsync(id);
            if (!successful)
            {
                return BadRequest(new { error = "Could not delete make." });
            }
            return RedirectToAction("Make");
        }

        public async Task<IActionResult> EditVehicleMake(Guid id)
        {
            var make = await _vehicleMakeService.GetByIdAsync(id);
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
                successful = await _vehicleMakeService.UpdateAsync(vehicleMake);
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
    }
}
