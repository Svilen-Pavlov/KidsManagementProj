using KidsManagement.Data.Models;
using KidsManagement.Services.External.CloudinaryService;
using KidsManagement.Services.Parents;
using KidsManagement.Services.Students;
using KidsManagement.ViewModels.Parents;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KidsManagement.Web.Controllers.Parents
{
    public class ParentsController : Controller
    {
        private readonly IStudentsService studentsService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IParentsService parentsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ParentsController(IStudentsService studentsService, ICloudinaryService cloudinaryService, IParentsService parentsService, UserManager<ApplicationUser> userManager)
        {
            this.studentsService = studentsService;
            this.cloudinaryService = cloudinaryService;
            this.parentsService = parentsService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = this.parentsService.GetAll();
            return await Task.Run(() => this.View(model));
        }

        public async Task<IActionResult> Create()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateParentInputModel model) //tozi model se vru6ta ot viewto
        {
            if (ModelState.IsValid == false)
            {
                return this.Redirect("/"); //invalid student create ERROR
            }

            var userAdminId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); // from https://stackoverflow.com/questions/30701006/how-to-get-the-current-logged-in-user-id-in-asp-net-core
            var parentId = await this.parentsService.CreateParent(model, userAdminId);


            return await Task.Run(() => RedirectToAction("Details", new { parentId = parentId }));
        }

        public async Task<IActionResult> Details(int parentId)
        {
            if (await this.parentsService.Exists(parentId) == false)
            {
                this.Redirect("/");
            }

            var parent = await this.parentsService.FindById(parentId);
            return this.View(parent);
        }
        public async Task<IActionResult> AddStudents(int parentId)
        {
            if (await this.parentsService.Exists(parentId) == false)
            {
                this.Redirect("/");
            }
            //TODO
            var parent = await this.parentsService.FindById(parentId);
            return this.View(parent);
        }
        

    }
}
