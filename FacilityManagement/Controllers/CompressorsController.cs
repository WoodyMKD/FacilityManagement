using FacilityManagement.DTOs;
using FacilityManagement.Web.Models.ViewModels;
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

        [HttpPost]
        public async Task<IActionResult> UpdateCompressorAjaxFormAsync(CompressorDetailsViewModel updatedModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_UpdateFormPartial", updatedModel);
            }

            // the client could validate this, but allowed for testing server errors
            if (updatedModel.Name.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                return PartialView("_UpdateFormPartial", updatedModel);
            }

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var serializedUpdatedModel = JsonConvert.SerializeObject(updatedModel);
            StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
            var response = await httpClient.PutAsync($"api/compressors", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return PartialView("_UpdateFormPartial", updatedModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpPost]
        public async Task<IActionResult> EditPartAjaxFormAsync(PartDetailsViewModel updatedModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditPartFormPartial", updatedModel);
            }

            // the client could validate this, but allowed for testing server errors
            if (updatedModel.Name.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                return PartialView("_EditPartFormPartial", updatedModel);
            }

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var serializedUpdatedModel = JsonConvert.SerializeObject(updatedModel);
            StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
            var response = await httpClient.PutAsync($"api/compressorParts", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return PartialView("_EditPartFormPartial", updatedModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpPost]
        public async Task<IActionResult> AddCompressorSystemAjaxFormAsync(CompressorSystemDetailsViewModel toAddModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AddCompressorSystemFormPartial", toAddModel);
            }

            // the client could validate this, but allowed for testing server errors
            if (toAddModel.Name.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                return PartialView("_AddCompressorSystemFormPartial", toAddModel);
            }

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var serializedUpdatedModel = JsonConvert.SerializeObject(toAddModel);
            StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
            var response = await httpClient.PostAsync($"api/compressorSystems", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return PartialView("_AddCompressorSystemFormPartial", toAddModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpPost]
        public async Task<IActionResult> EditCompressorSystemAjaxFormAsync(EditCompressorSystemDetailsViewModel toEditModel)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            int subTypeId = toEditModel.CompressorSubTypeId ?? -1;

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
                    return PartialView("_EditCompressorSystemFormPartial", toEditModel);
                }

                // the client could validate this, but allowed for testing server errors
                if (toEditModel.Name.Length < 3)
                {
                    ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                    return PartialView("_EditCompressorSystemFormPartial", toEditModel);
                }

                var serializedUpdatedModel = JsonConvert.SerializeObject(toEditModel);
                StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
                var response = await httpClient.PutAsync($"api/compressorSystems", content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode && subTypeId != -1)
                {
                    return PartialView("_EditCompressorSystemFormPartial", toEditModel);
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

        [HttpPost]
        public async Task<IActionResult> EditCompressorTypeAjaxFormAsync(EditCompressorTypeDetailsViewModel toEditModel)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            int compressorId = toEditModel.CompressorId ?? -1;

            // Get All CompressorSystems under given SubTypeId
            var systemsResponse = await httpClient.GetAsync($"api/compressorTypes/byCompressorId/{compressorId}").ConfigureAwait(false);
            if (systemsResponse.IsSuccessStatusCode)
            {
                var responseAsString = await systemsResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var compressorTypesModels = JsonConvert.DeserializeObject<CompressorSubTypeModel[]>(responseAsString);
                toEditModel.AllCompressorTypes = compressorTypesModels.ToList();

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, "Податоците не се валидни");
                    return PartialView("_EditCompressorTypeFormPartial", toEditModel);
                }

                // the client could validate this, but allowed for testing server errors
                if (toEditModel.Name.Length < 3)
                {
                    ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                    return PartialView("_EditCompressorTypeFormPartial", toEditModel);
                }

                var serializedUpdatedModel = JsonConvert.SerializeObject(toEditModel);
                StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
                var response = await httpClient.PutAsync($"api/compressorTypes", content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode && compressorId != -1)
                {
                    return PartialView("_EditCompressorTypeFormPartial", toEditModel);
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

        [HttpPost]
        public async Task<IActionResult> DeleteCompressorSystemAjaxFormAsync(DeleteCompressorSystemDetailsViewModel toDeleteModel)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            int subTypeId = toDeleteModel.CompressorSubTypeId ?? -1;

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
                    return PartialView("_DeleteCompressorSystemFormPartial", toDeleteModel);
                }

                return PartialView("_DeleteCompressorSystemFormPartial", toDeleteModel);
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
                    return PartialView("_DeleteCompressorTypeFormPartial", toDeleteModel);
                }

                return PartialView("_DeleteCompressorTypeFormPartial", toDeleteModel);
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
        public async Task<IActionResult> AddCompressorTypeAjaxFormAsync(CompressorTypeDetailsViewModel toAddModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AddCompressorTypeFormPartial", toAddModel);
            }

            // the client could validate this, but allowed for testing server errors
            if (toAddModel.Name.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                return PartialView("_AddCompressorTypeFormPartial", toAddModel);
            }

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var serializedUpdatedModel = JsonConvert.SerializeObject(toAddModel);
            StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
            var response = await httpClient.PostAsync($"api/compressorTypes", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return PartialView("_AddCompressorTypeFormPartial", toAddModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpPost]
        public async Task<IActionResult> AddPartAjaxFormAsync(PartDetailsViewModel toAddModel)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AddPartFormPartial", toAddModel);
            }

            // the client could validate this, but allowed for testing server errors
            if (toAddModel.Name.Length < 3)
            {
                ModelState.AddModelError(string.Empty, "Name should be longer than 2 chars");
                return PartialView("_AddPartFormPartial", toAddModel);
            }

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var serializedUpdatedModel = JsonConvert.SerializeObject(toAddModel);
            StringContent content = new StringContent(serializedUpdatedModel, Encoding.Unicode, "application/json");
            var response = await httpClient.PostAsync($"api/compressorParts", content).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return PartialView("_AddPartFormPartial", toAddModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpGet]
        public async Task<IActionResult> GetCompressorInfoByIdAjaxAsync(int id, bool returnPartial)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync($"api/compressors/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var compressorDetailsViewModel = JsonConvert.DeserializeObject<CompressorDetailsViewModel>(responseAsString);

                if (returnPartial)
                {
                    return PartialView("_CompressorTypesPartial", compressorDetailsViewModel);
                }
                else
                {
                    return Json(compressorDetailsViewModel);
                }
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

        public async Task<IActionResult> DeleteCompressorAjaxAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.DeleteAsync($"api/compressors/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                TempData["ToastType"] = "success";
                TempData["ToastTitle"] = "Успешно бришење";
                TempData["ToastMessage"] = "Успешно го избришавте компресорот";

                return Ok("Yay");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }
        
        public async Task<IActionResult> DeletePartAjaxAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.DeleteAsync($"api/compressorParts/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return Ok("Yay");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        public async Task<IActionResult> DeleteCompressorSystemAjaxAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.DeleteAsync($"api/compressorSystems/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                
                return Ok("Yay");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        public async Task<IActionResult> DeleteCompressorTypeAjaxAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.DeleteAsync($"api/compressorTypes/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return Ok("Yay");
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
