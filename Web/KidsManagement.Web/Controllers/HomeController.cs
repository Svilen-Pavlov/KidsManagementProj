using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KidsManagement.Web.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var entityId = 5;
            string entityName = "Students";
            var picURI=await this.cloudinaryService.UploadProfilePicASync(file, entityId,entityName);

            return this.Redirect("/");
        }
    }
}
