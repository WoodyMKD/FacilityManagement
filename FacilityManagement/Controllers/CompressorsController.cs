using FacilityManagement.DTOs;
using FacilityManagement.Web.Models.ViewModels;
using FacilityManagement.Web.Models.ViewModels.Modal;
using FacilityManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Controllers
{
    public class CompressorsController : Controller
    {
        private readonly IFacilityManagementHttpClient _facilityManagementHttpClient;
        private readonly IStringLocalizer<CompressorsController> _localizer;

        public CompressorsController(
            IStringLocalizer<CompressorsController> localizer,
            IFacilityManagementHttpClient facilityManagementHttpClient)
        {
            _localizer = localizer;
            _facilityManagementHttpClient = facilityManagementHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            //await WriteOutIdentityInformation();

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync("api/compressors").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var compressorIndexViewModel = new CompressorIndexViewModel()
                {
                    Compressors = JsonConvert.DeserializeObject<IList<CompressorModel>>(responseAsString).ToList()
                };

                return View(compressorIndexViewModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        public async Task<IActionResult> Details(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync($"api/compressors/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var compressorDetailsViewModel = JsonConvert.DeserializeObject<CompressorDetailsViewModel>(responseAsString);

                return View(compressorDetailsViewModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCompressorAjaxFormAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync($"api/compressors/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorDetailsViewModel = JsonConvert.DeserializeObject<CompressorDetailsViewModel>(responseAsString);

                return PartialView("FormModals/_UpdateInventoryObjectModal", compressorDetailsViewModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCompressorAjaxFormAsync(CompressorDetailsViewModel updatedModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                return PartialView("FormModals/_UpdateInventoryObjectModal", updatedModel);
            }

            // the client could validate this, but allowed for testing server errors
            if (updatedModel.Name.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                return PartialView("FormModals/_UpdateInventoryObjectModal", updatedModel);
            }

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var serializedUpdatedModel = JsonConvert.SerializeObject(updatedModel);
            StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
            var response = await httpClient.PutAsync($"api/compressors", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if (isAjaxRequest)
                {
                    return Content("success");
                }
                else
                {
                    return RedirectToAction("Index", "Compressors");
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
        public async Task<IActionResult> EditPartAjaxFormAsync(int id)
        {
            return PartialView("FormModals/_EditPartFormPartial", new EditPartDetailsViewModel() { PartId = id });
        }

        [HttpPost]
        public async Task<IActionResult> EditPartAjaxFormAsync(EditPartDetailsViewModel updatedModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                return PartialView("FormModals/_EditPartFormPartial", updatedModel);
            }

            // the client could validate this, but allowed for testing server errors
            if (updatedModel.Name.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                return PartialView("FormModals/_EditPartFormPartial", updatedModel);
            }

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var serializedUpdatedModel = JsonConvert.SerializeObject(updatedModel);
            StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
            var response = await httpClient.PutAsync($"api/compressorParts", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if (isAjaxRequest)
                {
                    return Content("success");
                }
                else
                {
                    return RedirectToAction("Index", "Compressors");
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
        public async Task<IActionResult> AddCompressorSystemAjaxFormAsync(int id)
        {
            return PartialView("FormModals/_AddCompressorSystemFormPartial", new CompressorSystemDetailsViewModel() { CompressorSubTypeId = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddCompressorSystemAjaxFormAsync(CompressorSystemDetailsViewModel toAddModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                return PartialView("FormModals/_AddCompressorSystemFormPartial", toAddModel);
            }

            // the client could validate this, but allowed for testing server errors
            if (toAddModel.Name.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                return PartialView("FormModals/_AddCompressorSystemFormPartial", toAddModel);
            }

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var serializedUpdatedModel = JsonConvert.SerializeObject(toAddModel);
            StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
            var response = await httpClient.PostAsync($"api/compressorSystems", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if (isAjaxRequest)
                {
                    return Content("success");
                }
                else
                {
                    return RedirectToAction("Index", "Compressors");
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
        public async Task<IActionResult> EditCompressorSystemAjaxFormAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var systemsResponse = await httpClient.GetAsync($"api/compressorSystems/bySubTypeId/{id}").ConfigureAwait(false);
            if (systemsResponse.IsSuccessStatusCode)
            {
                var responseAsString = await systemsResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorSystemModels = JsonConvert.DeserializeObject<CompressorSystemDetailsViewModel[]>(responseAsString);
                var compressorSystemsList = compressorSystemModels.ToList();

                return PartialView("FormModals/_EditCompressorSystemFormPartial",
                    new EditCompressorSystemDetailsViewModel() { CompressorSubTypeId = id, AllCompressorSystems = compressorSystemsList });
            }
            else if (systemsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    systemsResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {systemsResponse.ReasonPhrase}");
        }

        [HttpPost]
        public async Task<IActionResult> EditCompressorSystemAjaxFormAsync(EditCompressorSystemDetailsViewModel toEditModel)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            int subTypeId = toEditModel.CompressorSubTypeId;

            // Get All CompressorSystems under given SubTypeId
            var systemsResponse = await httpClient.GetAsync($"api/compressorSystems/bySubTypeId/{subTypeId}").ConfigureAwait(false);
            if (systemsResponse.IsSuccessStatusCode)
            {
                var responseAsString = await systemsResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorSystemModels = JsonConvert.DeserializeObject<CompressorSystemDetailsViewModel[]>(responseAsString);
                toEditModel.AllCompressorSystems = compressorSystemModels.ToList();

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                    return PartialView("FormModals/_EditCompressorSystemFormPartial", toEditModel);
                }

                // the client could validate this, but allowed for testing server errors
                if (toEditModel.Name.Length < 3)
                {
                    ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                    return PartialView("FormModals/_EditCompressorSystemFormPartial", toEditModel);
                }

                var serializedUpdatedModel = JsonConvert.SerializeObject(toEditModel);
                StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
                var response = await httpClient.PutAsync($"api/compressorSystems", content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                    if (isAjaxRequest)
                    {
                        return Content("success");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Compressors");
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
        public async Task<IActionResult> EditCompressorTypeAjaxFormAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();

            // Get All CompressorSystems under given SubTypeId
            var typesResponse = await httpClient.GetAsync($"api/compressorTypes/byCompressorId/{id}").ConfigureAwait(false);
            if (typesResponse.IsSuccessStatusCode)
            {
                var responseAsString = await typesResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorTypesModels = JsonConvert.DeserializeObject<CompressorSubTypeModel[]>(responseAsString);
                var compressorTypesList = compressorTypesModels.ToList();

                return PartialView("FormModals/_EditCompressorTypeFormPartial",
                    new EditCompressorTypeDetailsViewModel() { CompressorId = id, AllCompressorTypes = compressorTypesList }
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
        public async Task<IActionResult> EditCompressorTypeAjaxFormAsync(EditCompressorTypeDetailsViewModel toEditModel)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var compressorId = toEditModel.CompressorId;

            // Get All CompressorSystems under given SubTypeId
            var typesResponse = await httpClient.GetAsync($"api/compressorTypes/byCompressorId/{compressorId}").ConfigureAwait(false);
            if (typesResponse.IsSuccessStatusCode)
            {
                var responseAsString = await typesResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorTypesModels = JsonConvert.DeserializeObject<CompressorSubTypeModel[]>(responseAsString);
                toEditModel.AllCompressorTypes = compressorTypesModels.ToList();

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                    return PartialView("FormModals/_EditCompressorTypeFormPartial", toEditModel);
                }

                // the client could validate this, but allowed for testing server errors
                if (toEditModel.Name.Length < 3)
                {
                    ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                    return PartialView("FormModals/_EditCompressorTypeFormPartial", toEditModel);
                }

                var serializedUpdatedModel = JsonConvert.SerializeObject(toEditModel);
                StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
                var response = await httpClient.PutAsync($"api/compressorTypes", content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                    if (isAjaxRequest)
                    {
                        return Content("success");
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = toEditModel.CompressorId });
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
        public async Task<IActionResult> DeleteCompressorSystemAjaxFormAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();

            // Get All CompressorSystems under given SubTypeId
            var systemsResponse = await httpClient.GetAsync($"api/compressorSystems/bySubTypeId/{id}").ConfigureAwait(false);
            if (systemsResponse.IsSuccessStatusCode)
            {
                var responseAsString = await systemsResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorSystemModels = JsonConvert.DeserializeObject<CompressorSystemDetailsViewModel[]>(responseAsString);
                var compressorSystemsList = compressorSystemModels.ToList();

                return PartialView("FormModals/_DeleteCompressorSystemFormPartial",
                    new DeleteCompressorSystemDetailsViewModel() { CompressorSubTypeId = id, AllCompressorSystems = compressorSystemsList }
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
        public async Task<IActionResult> DeleteCompressorSystemAjaxFormAsync(DeleteCompressorSystemDetailsViewModel toDeleteModel)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            int subTypeId = toDeleteModel.CompressorSubTypeId;

            // Get All CompressorSystems under given SubTypeId
            var systemsResponse = await httpClient.GetAsync($"api/compressorSystems/bySubTypeId/{subTypeId}").ConfigureAwait(false);
            if (systemsResponse.IsSuccessStatusCode)
            {
                var responseAsString = await systemsResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorSystemModels = JsonConvert.DeserializeObject<CompressorSystemDetailsViewModel[]>(responseAsString);
                toDeleteModel.AllCompressorSystems = compressorSystemModels.ToList();

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                    return PartialView("FormModals/_DeleteCompressorSystemFormPartial", toDeleteModel);
                }

                bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if (isAjaxRequest)
                {
                    return Json(new
                    {
                        status = "success",
                        selectedID = toDeleteModel.CompressorSystemId
                    });
                }
                else
                {
                    return RedirectToAction("Index", "Compressors");
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
        public async Task<IActionResult> DeleteCompressorSystemAjaxAsync(int id)
        {
            var ModalDeleteModel = new ModalDelete()
            {
                Controller = "Compressors",
                Action = "DeleteCompressorSystemAjaxAsync",
                ModelId = id,
                Message = "Сигурен си?"
            };

            return PartialView("Modals/_ModalDelete", model: ModalDeleteModel);
        }

        [HttpPost, ActionName("DeleteCompressorSystemAjaxAsync")]
        public async Task<IActionResult> DeleteCompressorSystemAjaxAsyncConfirmed(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.DeleteAsync($"api/compressorSystems/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return Json(new
                {
                    status = "success",
                    deleteModel = "system",
                    toastMessage = "Успешно гo избришавте системот"
                });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCompressorTypeAjaxFormAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();

            // Get All CompressorSystems under given SubTypeId
            var systemsResponse = await httpClient.GetAsync($"api/compressorTypes/byCompressorId/{id}").ConfigureAwait(false);
            if (systemsResponse.IsSuccessStatusCode)
            {
                var responseAsString = await systemsResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorTypesModels = JsonConvert.DeserializeObject<CompressorSubTypeModel[]>(responseAsString);
                var compressorTypesList = compressorTypesModels.ToList();

                return PartialView("FormModals/_DeleteCompressorTypeFormPartial",
                    new DeleteCompressorTypeDetailsViewModel() { CompressorId = id, AllCompressorTypes = compressorTypesList }
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
        public async Task<IActionResult> DeleteCompressorTypeAjaxFormAsync(DeleteCompressorTypeDetailsViewModel toDeleteModel)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            int compressorId = toDeleteModel.CompressorId ?? -1;

            // Get All CompressorSystems under given SubTypeId
            var systemsResponse = await httpClient.GetAsync($"api/compressorTypes/byCompressorId/{compressorId}").ConfigureAwait(false);
            if (systemsResponse.IsSuccessStatusCode)
            {
                var responseAsString = await systemsResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorTypesModels = JsonConvert.DeserializeObject<CompressorSubTypeModel[]>(responseAsString);
                toDeleteModel.AllCompressorTypes = compressorTypesModels.ToList();

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                    return PartialView("FormModals/_DeleteCompressorTypeFormPartial", toDeleteModel);
                }

                bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if (isAjaxRequest)
                {
                    return Json(new {
                        status = "success",
                        selectedID = toDeleteModel.CompressorSubTypeId
                    });
                }
                else
                {
                    return RedirectToAction("Details", new { id = toDeleteModel.CompressorId });
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
        public async Task<IActionResult> AddCompressorTypeAjaxFormAsync(int id)
        {
            return PartialView("FormModals/_AddCompressorTypeFormPartial", new CompressorTypeDetailsViewModel() { CompressorId = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddCompressorTypeAjaxFormAsync(CompressorTypeDetailsViewModel toAddModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("FormModals/_AddCompressorTypeFormPartial", toAddModel);
            }

            // the client could validate this, but allowed for testing server errors
            if (toAddModel.Name.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                return PartialView("FormModals/_AddCompressorTypeFormPartial", toAddModel);
            }

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var serializedUpdatedModel = JsonConvert.SerializeObject(toAddModel);
            StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
            var response = await httpClient.PostAsync($"api/compressorTypes", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if (isAjaxRequest)
                {
                    return Content("success");
                }
                else
                {
                    return RedirectToAction("Index", "Compressors");
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
        public async Task<IActionResult> AddPartAjaxFormAsync(int id)
        {
            return PartialView("FormModals/_AddPartFormPartial", new PartDetailsViewModel() { CompressorSystemId = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddPartAjaxFormAsync(PartDetailsViewModel toAddModel)
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
            var response = await httpClient.PostAsync($"api/compressorParts", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if (isAjaxRequest)
                {
                    return Content("success");
                }
                else
                {
                    return RedirectToAction("Index", "Compressors");
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
        public async Task<IActionResult> GetCompressorTypesListPartial(int id)
        {
            string apiUrl = $"api/compressorTypes/byCompressorId/{id}";
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync(apiUrl).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var compressorTypesViewModel = new TypesViewModel {
                    CompressorId = id,
                    Types = JsonConvert.DeserializeObject<ICollection<CompressorSubTypeModel>>(responseAsString)
                };

                return PartialView("_CompressorTypesPartial", compressorTypesViewModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpGet]
        public async Task<IActionResult> GetCompressorSystemsAndPartsPartial(int id)
        {
            string apiUrl = $"api/compressorSystems/bySubTypeId/{id}";
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync(apiUrl).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var compressorSystemsViewModel = new SystemsViewModel
                {
                    CompressorTypeId = id,
                    Systems = JsonConvert.DeserializeObject<ICollection<CompressorSystemModel>>(responseAsString)
                };

                return PartialView("_CompressorSystemsPartial", compressorSystemsViewModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpGet]
        public async Task<IActionResult> GetCompressorGeneralInformationPartial(int id)
        {
            string apiUrl = $"api/compressors/{id}";
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync(apiUrl).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorDetailsViewModel = JsonConvert.DeserializeObject<CompressorDetailsViewModel>(responseAsString);
                return PartialView("_CompressorGeneralInformationPartial", compressorDetailsViewModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        public async Task<IActionResult> GetCompressorSystemsAndPartsBySubTypeIdAjaxAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync($"api/compressorSystems/bySubTypeId/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var compressorSystemModels = JsonConvert.DeserializeObject<CompressorSystemDetailsViewModel[]>(responseAsString);
                
                return PartialView("_CompressorSystemsPartial", compressorSystemModels);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteCompressorAjaxAsync(int id)
        {
            var ModalDeleteModel = new ModalDelete()
            {
                Controller = "Compressors",
                Action = "DeleteCompressorAjaxAsync",
                ModelId = id,
                Message = "Сигурен си комп?"
            };

            return PartialView("Modals/_ModalDelete", model: ModalDeleteModel);
        }

        [HttpPost, ActionName("DeleteCompressorAjaxAsync")]
        public async Task<IActionResult> DeleteCompressorAjaxAsyncConfirmed(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.DeleteAsync($"api/compressors/{id}").ConfigureAwait(false);
            var redirectToUrlStr = Url.Action("Index", "Compressors");

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                TempData["ToastType"] = "success";
                TempData["ToastTitle"] = "Успешно бришење";
                TempData["ToastMessage"] = "Успешно го избришавте компресорот";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                redirectToUrlStr = Url.Action("AccessDenied", "Authorization");
            }
            else
            {
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Грешка";
                TempData["ToastMessage"] = "Настана грешка, освежете ја страницата!";
            }

            return Json(new
            {
                redirectToUrl = redirectToUrlStr
            });
        }

        [HttpGet]
        public async Task<IActionResult> DeletePartAjaxAsync(int id)
        {
            var ModalDeleteModel = new ModalDelete()
            {
                Controller = "Compressors",
                Action = "DeletePartAjaxAsync",
                ModelId = id,
                Message = "Сигурен си?"
            };

            return PartialView("Modals/_ModalDelete", model: ModalDeleteModel);
        }

        [HttpPost, ActionName("DeletePartAjaxAsync")]
        public async Task<IActionResult> DeletePartAjaxAsyncConfirmed(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.DeleteAsync($"api/compressorParts/{id}").ConfigureAwait(false);

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

        [HttpGet]
        public async Task<IActionResult> DeleteCompressorTypeAjaxAsync(int id)
        {
            var ModalDeleteModel = new ModalDelete()
            {
                Controller = "Compressors",
                Action = "DeleteCompressorTypeAjaxAsync",
                ModelId = id,
                Message = "Сигурен си?"
            };

            return PartialView("Modals/_ModalDelete", model: ModalDeleteModel);
        }

        [HttpPost, ActionName("DeleteCompressorTypeAjaxAsync")]
        public async Task<IActionResult> DeleteCompressorTypeAjaxAsyncConfirmed(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.DeleteAsync($"api/compressorTypes/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return Json(new
                {
                    status = "success",
                    deleteModel = "type",
                    toastMessage = "Успешно гo избришавте типот"
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
