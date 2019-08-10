﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using HATEOAS.DomainModel;
//using HATEOAS.Repositorys;
//using HATEOAS.ViewModes;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc;
//
//namespace HATEOAS.Controllers
//{
//
//    public class BaseController<T> : Controller
//    {
//
//    }
//
//    [AllowAnonymous]
//    [Route("api/sales/[controller]")]
//    public class VehicleController : BaseController<VehicleController>
//    {
//        private readonly IVehicleRepository _vehicleRepository;
//        private readonly IUrlHelper _urlHelper;
//
//        public VehicleController(
//            IVehicleRepository vehicleRepository,
//            IUrlHelper urlHelper)
//        {
//            _vehicleRepository = vehicleRepository;
//            this._urlHelper = urlHelper;
//        }
//
//        [HttpGet]
//        [Route("{id}", Name = "GetVehicle")]
//        public async Task<IActionResult> Get(int id)
//        {
//            var item = await _vehicleRepository.GetSingleAsync(id);
//            if (item == null)
//            {
//                return NotFound();
//            }
//            var vehicleVm = Mapper.Map<VehicleViewModel>(item);
//            return Ok(CreateLinksForVehicle(vehicleVm));
//        }
//
//        [HttpPost]
//        public async Task<IActionResult> Post([FromBody] VehicleViewModel vehicleVm)
//        {
//            if (vehicleVm == null)
//            {
//                return BadRequest();
//            }
//
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//
//            var newItem = Mapper.Map<Vehicle>(vehicleVm);
//            _vehicleRepository.Add(newItem);
//            if (!await UnitOfWork.SaveAsync())
//            {
//                return StatusCode(500, "保存时出错");
//            }
//
//            var vm = Mapper.Map<VehicleViewModel>(newItem);
//
//            return CreatedAtRoute("GetVehicle", new { id = vm.Id }, CreateLinksForVehicle(vm));
//        }
//
//        [HttpGet(Name = "GetAllVehicles")]
//        public async Task<IActionResult> GetAll()
//        {
//            var items = await _vehicleRepository.All.ToListAsync();
//            var results = Mapper.Map<IEnumerable<VehicleViewModel>>(items);
//            results = results.Select(CreateLinksForVehicle);
//            var wrapper = new LinkedCollectionResourceWrapperViewModel<VehicleViewModel>(results);
//            return Ok(CreateLinksForVehicle(wrapper));
//        }`
//
//        [HttpPut("{id}", Name = "UpdateVehicle")]
//        public async Task<IActionResult> Put(int id, [FromBody] VehicleViewModel vehicleVm)
//        {
//            if (vehicleVm == null)
//            {
//                return BadRequest();
//            }
//
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//            var dbItem = await _vehicleRepository.GetSingleAsync(id);
//            if (dbItem == null)
//            {
//                return NotFound();
//            }
//            Mapper.Map(vehicleVm, dbItem);
//            _vehicleRepository.Update(dbItem);
//            if (!await UnitOfWork.SaveAsync())
//            {
//                return StatusCode(500, "保存时出错");
//            }
//
//            return NoContent();
//        }
//
//        [HttpPatch("{id}", Name = "PartiallyUpdateVehicle")]
//        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<VehicleViewModel> patchDoc)
//        {
//            if (patchDoc == null)
//            {
//                return BadRequest();
//            }
//            var dbItem = await _vehicleRepository.GetSingleAsync(id);
//            if (dbItem == null)
//            {
//                return NotFound();
//            }
//            var toPatchVm = Mapper.Map<VehicleViewModel>(dbItem);
//            patchDoc.ApplyTo(toPatchVm, ModelState);
//
//            TryValidateModel(toPatchVm);
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//
//            Mapper.Map(toPatchVm, dbItem);
//
//            if (!await UnitOfWork.SaveAsync())
//            {
//                return StatusCode(500, "更新时出错");
//            }
//
//            return NoContent();
//        }
//
//        [HttpDelete("{id}", Name = "DeleteVehicle")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var model = await _vehicleRepository.GetSingleAsync(id);
//            if (model == null)
//            {
//                return NotFound();
//            }
//            _vehicleRepository.Delete(model);
//            if (!await UnitOfWork.SaveAsync())
//            {
//                return StatusCode(500, "删除时出错");
//            }
//            return NoContent();
//        }
//
//        private VehicleViewModel CreateLinksForVehicle(VehicleViewModel vehicle)
//        {
//            vehicle.Links.Add(
//                new LinkViewModel(
//                    href: _urlHelper.Link("GetVehicle", new { id = vehicle.Id }),
//                    rel: "self",
//                    method: "GET"));
//
//            vehicle.Links.Add(
//                new LinkViewModel(
//                    href: _urlHelper.Link("UpdateVehicle", new { id = vehicle.Id }),
//                    rel: "update_vehicle",
//                    method: "PUT"));
//
//            vehicle.Links.Add(
//            new LinkViewModel(
//                href: _urlHelper.Link("PartiallyUpdateVehicle", new { id = vehicle.Id }),
//                rel: "partially_update_vehicle",
//                method: "PATCH"));
//
//            vehicle.Links.Add(
//            new LinkViewModel(
//                href: _urlHelper.Link("DeleteVehicle", new { id = vehicle.Id }),
//                rel: "delete_vehicle",
//                method: "DELETE"));
//
//            return vehicle;
//        }
//
//        private LinkedCollectionResourceWrapperViewModel<VehicleViewModel> CreateLinksForVehicle(
//            LinkedCollectionResourceWrapperViewModel<VehicleViewModel> vehiclesWrapper)
//        {
//            vehiclesWrapper.Links.Add(
//                new LinkViewModel(_urlHelper.Link("GetAllVehicles", new { }),
//                    "self",
//                    "GET"
//                ));
//
//            return vehiclesWrapper;
//        }
//    }
//}
