using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using FacilityManagement.Web.Services;
using FacilityManagement.Web.Models.ViewModels;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;

namespace FacilityManagement.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // Низок приоритет:
        // TODO: Менаџирање профил со IdentityServer4 API (Низок приоритет)
        // TODO: Да се промени стилот за најава и регистрација - како Нептун (Низок приоритет)

        // Среден приоритет:
        // TODO: Localization на IdentityServer

        // Висок приоритет:
        // TODO: Тргни коментар за API да бара авторизација

        private readonly IFacilityManagementHttpClient _facilityManagementHttpClient;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(
            IStringLocalizer<HomeController> localizer,
            IFacilityManagementHttpClient facilityManagementHttpClient)
        {
            _localizer = localizer;
            _facilityManagementHttpClient = facilityManagementHttpClient;
        }

        [DefaultBreadcrumb("Home")]
        public IActionResult Index()
        {
            //await WriteOutIdentityInformation();

            return View();
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
