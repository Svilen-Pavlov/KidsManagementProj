using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KidsManagement.Web.Models;
using Microsoft.AspNetCore.Http;
using KidsManagement.Services.External.CloudinaryService;

namespace KidsManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICloudinaryService cloudinaryService;

        public HomeController(ILogger<HomeController> logger, ICloudinaryService cloudinaryService)
        {

            _logger = logger;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult Index()
        {
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

       
    }
}
