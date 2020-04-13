using KidsManagement.Services.External.CloudinaryService;
using KidsManagement.Services.Students;
using KidsManagement.ViewModels.Students;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KidsManagement.Web.Controllers.Students
{
    public class StudentsController:Controller
    {
        private readonly IStudentsService studentsService;
        private readonly ICloudinaryService cloudinaryService;

        public StudentsController(IStudentsService studentsService,ICloudinaryService cloudinaryService)
        {
            this.studentsService = studentsService;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> Index()
        {

            return await Task.Run(() => View());
        }
        public async Task<IActionResult> Details(int studentId)
        {
            if (await this.studentsService.StudentExists(studentId)==false)
            {
                this.Redirect("/");
            }

            var studentModel = await this.studentsService.FindById(studentId);
            return this.View(studentModel);
        }

        public async Task<IActionResult> Create()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStudentInputModel input)
        {
            if (ModelState.IsValid==false)
            {
                return this.Redirect("/"); //invalide student create ERROR
            }
            var studentId = await this.studentsService.CreateStudent(input);

            return this.RedirectToAction("Details", studentId);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int studentId)
        {
            if (file==null)
            {
                return this.Redirect("/"); //FILE IS NULL ERROR
            }
            var a =this.RouteData.Values;
            var entityId = studentId;
            string entityName = "Students";
            var picURI = await this.cloudinaryService.UploadProfilePicASync(file);

            return this.RedirectToAction("Details", entityId);
        }
    }
}
