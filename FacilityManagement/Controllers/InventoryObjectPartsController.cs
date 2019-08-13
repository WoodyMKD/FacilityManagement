using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FacilityManagement.Web.Models.ViewModels;
using FacilityManagement.Web.Models.ViewModels.Modal;
using FacilityManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace FacilityManagement.Web.Controllers
{
    public class InventoryObjectPartsController : Controller
    {
        private readonly IFacilityManagementHttpClient _facilityManagementHttpClient;
        private readonly IStringLocalizer<InventoryObjectPartsController> _localizer;

        public InventoryObjectPartsController(
            IStringLocalizer<InventoryObjectPartsController> localizer,
            IFacilityManagementHttpClient facilityManagementHttpClient)
        {
            _localizer = localizer;
            _facilityManagementHttpClient = facilityManagementHttpClient;
        }

        [HttpGet]
        public async Task<IActionResult> AddInventoryObjectPartAjaxFormAsync(int id)
        {
            return PartialView("FormModals/_AddPartFormPartial", new PartDetailsViewModel() { InventoryObjectSystemId = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddInventoryObjectPartAjaxFormAsync(PartDetailsViewModel toAddModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                return PartialView("FormModals/_AddPartFormPartial", toAddModel);
            }

            // the client could validate this, but allowed for testing server errors
            if (toAddModel.Name.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                return PartialView("FormModals/_AddPartFormPartial", toAddModel);
            }

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var serializedUpdatedModel = JsonConvert.SerializeObject(toAddModel);
            StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
            var response = await httpClient.PostAsync($"api/inventory/parts", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if (isAjaxRequest)
                {
                    return Content("success");
                }
                else
                {
                    return RedirectToAction("Index", "InventoryObjects");
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpGet]
        public async Task<IActionResult> EditInventoryObjectPartAjaxFormAsync(int id)
        {
            return PartialView("FormModals/_UpdatePartFormPartial", new UpdatePartDetailsViewModel() { InventoryObjectPartId = id });
        }

        [HttpPost]
        public async Task<IActionResult> EditInventoryObjectPartAjaxFormAsync(UpdatePartDetailsViewModel updatedModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                return PartialView("FormModals/_UpdatePartFormPartial", updatedModel);
            }

            // the client could validate this, but allowed for testing server errors
            if (updatedModel.Name.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                return PartialView("FormModals/_UpdatePartFormPartial", updatedModel);
            }

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var serializedUpdatedModel = JsonConvert.SerializeObject(updatedModel);
            StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
            var response = await httpClient.PutAsync($"api/inventory/parts", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if (isAjaxRequest)
                {
                    return Content("success");
                }
                else
                {
                    return RedirectToAction("Index", "InventoryObjects");
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteInventoryObjectPartAjaxAsync(int id)
        {
            var ModalDeleteModel = new ModalDelete()
            {
                Controller = "InventoryObjectParts",
                Action = "DeleteInventoryObjectPartAjaxAsync",
                ModelId = id,
                Message = "Сигурен си?"
            };

            return PartialView("Modals/_ModalDelete", model: ModalDeleteModel);
        }

        [HttpPost, ActionName("DeleteInventoryObjectPartAjaxAsync")]
        public async Task<IActionResult> DeleteInventoryObjectPartAjaxAsyncConfirmed(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.DeleteAsync($"api/inventory/parts/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return Json(new
                {
                    status = "success",
                    deleteModel = "part",
                    toastMessage = "Успешно ја избришавте компонентата"
                });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }
    }
}