using FacilityManagement.DTOs;
using FacilityManagement.Web.Models.ViewModels;
using FacilityManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Controllers
{
    public class CompressorsController: Controller
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
    }
}
