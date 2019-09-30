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
    public class InventoryObjectTypesController : Controller
    {
        private readonly IFacilityManagementHttpClient _facilityManagementHttpClient;
        private readonly IStringLocalizer<InventoryObjectTypesController> _localizer;

        public InventoryObjectTypesController(
            IStringLocalizer<InventoryObjectTypesController> localizer,
            IFacilityManagementHttpClient facilityManagementHttpClient)
        {
            _localizer = localizer;
            _facilityManagementHttpClient = facilityManagementHttpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventoryObjectTypesListPartial(int id)
        {
            string apiUrl = $"api/inventory/types/byInventoryObjectId/{id}";
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync(apiUrl).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var compressorTypesViewModel = new TypesViewModel
                {
                    InventoryObjectId = id,
                    Types = JsonConvert.DeserializeObject<ICollection<InventoryObjectTypeDTO>>(responseAsString)
                };

                return PartialView("_TypesPartial", compressorTypesViewModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpGet]
        public async Task<IActionResult> AddInventoryObjectTypeAjaxFormAsync(int id)
        {
            return PartialView("FormModals/_AddTypeFormPartial", new TypeDetailsViewModel() { InventoryObjectId = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddInventoryObjectTypeAjaxFormAsync(TypeDetailsViewModel toAddModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("FormModals/_AddTypeFormPartial", toAddModel);
            }

            // the client could validate this, but allowed for testing server errors
            if (toAddModel.Name.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                return PartialView("FormModals/_AddTypeFormPartial", toAddModel);
            }

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var serializedUpdatedModel = JsonConvert.SerializeObject(toAddModel);
            StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
            var response = await httpClient.PostAsync($"api/inventory/types", content).ConfigureAwait(false);

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
        public async Task<IActionResult> EditInventoryObjectTypeAjaxFormAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();

            // Get All CompressorSystems under given SubTypeId
            var typesResponse = await httpClient.GetAsync($"api/inventory/types/byInventoryObjectId/{id}").ConfigureAwait(false);
            if (typesResponse.IsSuccessStatusCode)
            {
                var responseAsString = await typesResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorTypesModels = JsonConvert.DeserializeObject<InventoryObjectTypeDTO[]>(responseAsString);
                var compressorTypesList = compressorTypesModels.ToList();

                return PartialView("FormModals/_UpdateTypeFormPartial",
                    new UpdateTypeDetailsViewModel() { InventoryObjectId = id, AllTypes = compressorTypesList }
                );
            }
            else if (typesResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    typesResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {typesResponse.ReasonPhrase}");
        }

        [HttpPost]
        public async Task<IActionResult> EditInventoryObjectTypeAjaxFormAsync(UpdateTypeDetailsViewModel toEditModel)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var compressorId = toEditModel.InventoryObjectId;

            // Get All CompressorSystems under given SubTypeId
            var typesResponse = await httpClient.GetAsync($"api/inventory/types/byInventoryObjectId/{compressorId}").ConfigureAwait(false);
            if (typesResponse.IsSuccessStatusCode)
            {
                var responseAsString = await typesResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorTypesModels = JsonConvert.DeserializeObject<InventoryObjectTypeDTO[]>(responseAsString);
                toEditModel.AllTypes = compressorTypesModels.ToList();

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                    return PartialView("FormModals/_UpdateTypeFormPartial", toEditModel);
                }

                // the client could validate this, but allowed for testing server errors
                if (toEditModel.Name.Length < 3)
                {
                    ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                    return PartialView("FormModals/_UpdateTypeFormPartial", toEditModel);
                }

                var serializedUpdatedModel = JsonConvert.SerializeObject(toEditModel);
                StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
                var response = await httpClient.PutAsync($"api/inventory/types", content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                    if (isAjaxRequest)
                    {
                        return Content("success");
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = toEditModel.InventoryObjectId });
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                        response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("AccessDenied", "Authorization");
                }

                throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
            }
            else if (typesResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    typesResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {typesResponse.ReasonPhrase}");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteInventoryObjectTypeAjaxFormAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();

            // Get All CompressorSystems under given SubTypeId
            var systemsResponse = await httpClient.GetAsync($"api/inventory/types/byInventoryObjectId/{id}").ConfigureAwait(false);
            if (systemsResponse.IsSuccessStatusCode)
            {
                var responseAsString = await systemsResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorTypesModels = JsonConvert.DeserializeObject<InventoryObjectTypeDTO[]>(responseAsString);
                var compressorTypesList = compressorTypesModels.ToList();

                return PartialView("FormModals/_DeleteTypeFormPartial",
                    new DeleteTypeDetailsViewModel() { InventoryObjectId = id, AllTypes = compressorTypesList }
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
        public async Task<IActionResult> DeleteInventoryObjectTypeAjaxFormAsync(DeleteTypeDetailsViewModel toDeleteModel)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            int compressorId = toDeleteModel.InventoryObjectId;

            // Get All CompressorSystems under given SubTypeId
            var systemsResponse = await httpClient.GetAsync($"api/inventory/types/byInventoryObjectId/{compressorId}").ConfigureAwait(false);
            if (systemsResponse.IsSuccessStatusCode)
            {
                var responseAsString = await systemsResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorTypesModels = JsonConvert.DeserializeObject<InventoryObjectTypeDTO[]>(responseAsString);
                toDeleteModel.AllTypes = compressorTypesModels.ToList();

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                    return PartialView("FormModals/_DeleteTypeFormPartial", toDeleteModel);
                }

                bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if (isAjaxRequest)
                {
                    return Json(new
                    {
                        status = "success",
                        selectedID = toDeleteModel.InventoryObjectTypeId
                    });
                }
                else
                {
                    return RedirectToAction("Details", new { id = toDeleteModel.InventoryObjectId });
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
        public async Task<IActionResult> DeleteInventoryObjectTypeAjaxAsync(int id)
        {
            var ModalDeleteModel = new ModalDelete()
            {
                Controller = "InventoryObjectTypes",
                Action = "DeleteInventoryObjectTypeAjaxAsync",
                ModelId = id,
                Message = ""
            };

            return PartialView("Modals/_ModalDelete", model: ModalDeleteModel);
        }

        [HttpPost, ActionName("DeleteInventoryObjectTypeAjaxAsync")]
        public async Task<IActionResult> DeleteInventoryObjectTypeAjaxAsyncConfirmed(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.DeleteAsync($"api/inventory/types/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return Json(new
                {
                    status = "success",
                    deleteModel = "type",
                    toastMessage = "The type was successfully deleted"
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