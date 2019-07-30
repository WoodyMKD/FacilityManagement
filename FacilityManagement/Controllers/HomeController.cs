using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FacilityManagement.Web.Models;
using FacilityManagement.Web.Models.ViewModels;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using FacilityManagement.Web.Services;
using FacilityManagement.API.Models;
using System.Net.Http;

namespace FacilityManagement.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // Листа за целата апликација:
        // TODO: Целосна интеграција со AdminLTE, сега е hard-coded (_Layout.cshtml)
        // TODO: Да се додаде image uploader на слика менување профил (Identity)
        // TODO: DTO, сега го користам од API моделот...
        // TODO: Да се среди преводот и да се средат копчињата за промена на јазикот
        // TODO: Logout e broken i najava po registracicja... IDentityServer4
        // TODO: При регистрација немора слика... дај му default (Identity)
        // TODO: Да се направи Account System со информации за корисничко име и работна позиција
        // TODO: Да се додаде нова страница за најава
        // TODO: Почетната страница да се стилизира со bootstrap и dynatables (Home/Index.cshtml)
        
        private readonly IFacilityManagementHttpClient _facilityManagementHttpClient;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(
            IStringLocalizer<HomeController> localizer,
            IFacilityManagementHttpClient facilityManagementHttpClient)
        {
            _localizer = localizer;
            _facilityManagementHttpClient = facilityManagementHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            //await WriteOutIdentityInformation();

            var httpClient = await _facilityManagementHttpClient.GetClient();
            var response = await httpClient.GetAsync("api/InventoryObjects").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var responseObject = response.Content.ReadAsAsync<IEnumerable<InventoryObject>>();
                responseObject.Wait();

                var homeViewModel = new HomeViewModel()
                {
                    InventoryObjects = responseObject.Result.ToList()
                };

                return View(homeViewModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }
            
            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        public IActionResult Details(int id)
        {
            //var invObject = _invObjectRepository.GetInventoryObjectById(id);
            //if (invObject == null)
                return NotFound();

            //return View(invObject);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public async Task WriteOutIdentityInformation()
        {
            // get the saved identity token
            var identityToken = await HttpContext
                .GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            // write it out
            Debug.WriteLine($"Identity token: {identityToken}");

            // write out the user claims
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }

        public async Task Logout()
        {
            // Clears the  local cookie ("Cookies" must match name from scheme)
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }
    }
}
