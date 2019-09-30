using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FacilityManagement.DTOs;
using FacilityManagement.Web.Models.ViewModels;
using FacilityManagement.Web.Models.ViewModels.Modal;
using FacilityManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace FacilityManagement.Web.Controllers
{
    public class InventoryObjectSystemsController : Controller
    {
        private readonly IFacilityManagementHttpClient _facilityManagementHttpClient;
        private readonly IStringLocalizer<InventoryObjectSystemsController> _localizer;

        public InventoryObjectSystemsController(
            IStringLocalizer<InventoryObjectSystemsController> localizer,
            IFacilityManagementHttpClient facilityManagementHttpClient)
        {
            _localizer = localizer;
            _facilityManagementHttpClient = facilityManagementHttpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventoryObjectSystemsAndPartsPartial(int id)
        {
            string apiUrl = $"api/inventory/systems/byTypeId/{id}";
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync(apiUrl).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var compressorSystemsViewModel = new SystemsViewModel
                {
                    InventoryObjectTypeId = id,
                    Systems = JsonConvert.DeserializeObject<ICollection<InventoryObjectSystemDTO>>(responseAsString)
                };

                return PartialView("_SystemsPartial", compressorSystemsViewModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        public async Task<IActionResult> GetInventoryObjectSystemsAndPartsBySubTypeIdAjaxAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync($"api/inventory/systems/byTypeId/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var systemModels = JsonConvert.DeserializeObject<SystemDetailsViewModel[]>(responseAsString);

                return PartialView("_SystemsPartial", systemModels);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpGet]
        public async Task<IActionResult> AddInventoryObjectSystemAjaxFormAsync(int id)
        {
            return PartialView("FormModals/_AddSystemFormPartial", new SystemDetailsViewModel() { InventoryObjectTypeId = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddInventoryObjectSystemAjaxFormAsync(SystemDetailsViewModel toAddModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                return PartialView("FormModals/_AddSystemFormPartial", toAddModel);
            }

            // the client could validate this, but allowed for testing server errors
            if (toAddModel.Name.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                return PartialView("FormModals/_AddSystemFormPartial", toAddModel);
            }

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var serializedUpdatedModel = JsonConvert.SerializeObject(toAddModel);
            StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
            var response = await httpClient.PostAsync($"api/inventory/systems", content).ConfigureAwait(false);

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
        public async Task<IActionResult> EditInventoryObjectSystemAjaxFormAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var systemsResponse = await httpClient.GetAsync($"api/inventory/systems/byTypeId/{id}").ConfigureAwait(false);
            if (systemsResponse.IsSuccessStatusCode)
            {
                var responseAsString = await systemsResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var systemModels = JsonConvert.DeserializeObject<SystemDetailsViewModel[]>(responseAsString);
                var compressorSystemsList = systemModels.ToList();

                return PartialView("FormModals/_UpdateSystemFormPartial",
                    new UpdateSystemDetailsViewModel() { InventoryObjectTypeId = id, AllSystems = compressorSystemsList });
            }
            else if (systemsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    systemsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {systemsResponse.ReasonPhrase}");
        }

        [HttpPost]
        public async Task<IActionResult> EditInventoryObjectSystemAjaxFormAsync(UpdateSystemDetailsViewModel toEditModel)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            int subTypeId = toEditModel.InventoryObjectTypeId;

            // Get All CompressorSystems under given SubTypeId
            var systemsResponse = await httpClient.GetAsync($"api/inventory/systems/byTypeId/{subTypeId}").ConfigureAwait(false);
            if (systemsResponse.IsSuccessStatusCode)
            {
                var responseAsString = await systemsResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var systemModels = JsonConvert.DeserializeObject<SystemDetailsViewModel[]>(responseAsString);
                toEditModel.AllSystems = systemModels.ToList();

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                    return PartialView("FormModals/_UpdateSystemFormPartial", toEditModel);
                }

                // the client could validate this, but allowed for testing server errors
                if (toEditModel.Name.Length < 3)
                {
                    ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                    return PartialView("FormModals/_UpdateSystemFormPartial", toEditModel);
                }

                var serializedUpdatedModel = JsonConvert.SerializeObject(toEditModel);
                StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
                var response = await httpClient.PutAsync($"api/inventory/systems", content).ConfigureAwait(false);

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
            else if (systemsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    systemsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {systemsResponse.ReasonPhrase}");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteInventoryObjectSystemAjaxFormAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();

            // Get All CompressorSystems under given SubTypeId
            var systemsResponse = await httpClient.GetAsync($"api/inventory/systems/byTypeId/{id}").ConfigureAwait(false);
            if (systemsResponse.IsSuccessStatusCode)
            {
                var responseAsString = await systemsResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var systemModels = JsonConvert.DeserializeObject<SystemDetailsViewModel[]>(responseAsString);
                var compressorSystemsList = systemModels.ToList();

                return PartialView("FormModals/_DeleteSystemFormPartial",
                    new DeleteSystemDetailsViewModel() { InventoryObjectTypeId = id, AllSystems = compressorSystemsList }
                );
            }
            else if (systemsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    systemsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }
            else
            {
                throw new Exception($"A problem happened while calling the API: {systemsResponse.ReasonPhrase}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteInventoryObjectSystemAjaxFormAsync(DeleteSystemDetailsViewModel toDeleteModel)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            int subTypeId = toDeleteModel.InventoryObjectTypeId;

            // Get All CompressorSystems under given SubTypeId
            var systemsResponse = await httpClient.GetAsync($"api/inventory/systems/byTypeId/{subTypeId}").ConfigureAwait(false);
            if (systemsResponse.IsSuccessStatusCode)
            {
                var responseAsString = await systemsResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var systemModels = JsonConvert.DeserializeObject<SystemDetailsViewModel[]>(responseAsString);
                toDeleteModel.AllSystems = systemModels.ToList();

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                    return PartialView("FormModals/_DeleteSystemFormPartial", toDeleteModel);
                }

                bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if (isAjaxRequest)
                {
                    return Json(new
                    {
                        status = "success",
                        selectedID = toDeleteModel.InventoryObjectSystemId
                    });
                }
                else
                {
                    return RedirectToAction("Index", "InventoryObjects");
                }
            }
            else if (systemsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    systemsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }
            else
            {
                throw new Exception($"A problem happened while calling the API: {systemsResponse.ReasonPhrase}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteInventoryObjectSystemAjaxAsync(int id)
        {
            var ModalDeleteModel = new ModalDelete()
            {
                Controller = "InventoryObjectSystems",
                Action = "DeleteInventoryObjectSystemAjaxAsync",
                ModelId = id,
                Message = ""
            };

            return PartialView("Modals/_ModalDelete", model: ModalDeleteModel);
        }

        [HttpPost, ActionName("DeleteInventoryObjectSystemAjaxAsync")]
        public async Task<IActionResult> DeleteInventoryObjectSystemAjaxAsyncConfirmed(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.DeleteAsync($"api/inventory/systems/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return Json(new
                {
                    status = "success",
                    deleteModel = "system",
                    toastMessage = "The system was successfully deleted"
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