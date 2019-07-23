using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FacilityManagement.Web.Models;
using FacilityManagement.Web.Models.Repositories;
using FacilityManagement.Web.Models.ViewModels;

namespace FacilityManagement.Controllers
{
    public class HomeController : Controller
    {
        // Листа за целата апликација:
        // TODO: Целосна интеграција со AdminLTE, сега е hard-coded (_Layout.cshtml)
        // TODO: Да се додаде image uploader на слика менување профил (Identity)
        // TODO: При регистрација немора слика... дај му default (Identity)
        // TODO: Да се направи Account System со информации за корисничко име и работна позиција
        // TODO: Да се додаде нова страница за најава
        // TODO: Почетната страница да се стилизира со bootstrap и dynatables (Home/Index.cshtml)

        private readonly IInventoryRepository _invObjectRepository;

        public HomeController(IInventoryRepository invObjectRepository)
        {
            _invObjectRepository = invObjectRepository;
        }

        public IActionResult Index()
        {
            var invObjects = _invObjectRepository.GetAllInventoryObjects().OrderBy(p => p.Name);

            var homeViewModel = new HomeViewModel()
            {
                InventoryObjects = invObjects.ToList()
            };

            return View(homeViewModel);
        }

        public IActionResult Details(int id)
        {
            var invObject = _invObjectRepository.GetInventoryObjectById(id);
            if (invObject == null)
                return NotFound();

            return View(invObject);
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
    }
}
