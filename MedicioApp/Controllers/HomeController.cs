using Medicio.Business.Services.Abstracts;
using MedicioApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedicioApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDoctorService _doctorService;
        public HomeController(ILogger<HomeController> logger, IDoctorService doctorService)
        {
            _logger = logger;
            _doctorService = doctorService;
        }

        public IActionResult Index()
        {
            var doctors = _doctorService.GetAllDoctors();
            return View(doctors );
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