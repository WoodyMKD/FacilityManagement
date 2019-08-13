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
    public class InventoryObjectsController : Controller
    {
        private readonly IFacilityManagementHttpClient _facilityManagementHttpClient;
        private readonly IStringLocalizer<InventoryObjectsController> _localizer;

        public InventoryObjectsController(
            IStringLocalizer<InventoryObjectsController> localizer,
            IFacilityManagementHttpClient facilityManagementHttpClient)
        {
            _localizer = localizer;
            _facilityManagementHttpClient = facilityManagementHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            //await WriteOutIdentityInformation();

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync("api/inventory").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var indexViewModel = new IndexViewModel()
                {
                    InventoryObjects = JsonConvert.DeserializeObject<IList<InventoryObjectDTO>>(responseAsString).ToList()
                };

                return View(indexViewModel);
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
            var response = await httpClient.GetAsync($"api/inventory/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var detailsViewModel = JsonConvert.DeserializeObject<DetailsViewModel>(responseAsString);

                return View(detailsViewModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateInventoryObjectAjaxFormAsync(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync($"api/inventory/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var detailsViewModel = JsonConvert.DeserializeObject<DetailsViewModel>(responseAsString);

                return PartialView("FormModals/_UpdateInventoryObjectModal", detailsViewModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInventoryObjectAjaxFormAsync(DetailsViewModel updatedModel)
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
            var response = await httpClient.PutAsync($"api/inventory", content).ConfigureAwait(false);

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
        public async Task<IActionResult> GetInventoryObjectGeneralInformationPartial(int id)
        {
            string apiUrl = $"api/inventory/{id}";
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync(apiUrl).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var detailsViewModel = JsonConvert.DeserializeObject<DetailsViewModel>(responseAsString);
                return PartialView("_GeneralInformationPartial", detailsViewModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteInventoryObjectAjaxAsync(int id)
        {
            var ModalDeleteModel = new ModalDelete()
            {
                Controller = "InventoryObjects",
                Action = "DeleteInventoryObjectAjaxAsync",
                ModelId = id,
                Message = "Сигурен си комп?"
            };

            return PartialView("Modals/_ModalDelete", model: ModalDeleteModel);
        }

        [HttpPost, ActionName("DeleteInventoryObjectAjaxAsync")]
        public async Task<IActionResult> DeleteInventoryObjectAjaxAsyncConfirmed(int id)
        {
            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.DeleteAsync($"api/inventory/{id}").ConfigureAwait(false);
            var redirectToUrlStr = Url.Action("Index", "InventoryObjects");

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
    }
}
