using KidsManagement.Services.Students;
using KidsManagement.ViewModels.Students;
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

        public StudentsController(IStudentsService studentsService)
        {
            this.studentsService = studentsService;
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

            var student = await this.studentsService.FindById(studentId);
            return this.View(student);
        }

        public async Task<IActionResult> Create()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStudentInputModel input)
        {
            var studentId = await this.studentsService.CreateStudent(input);

            return this.RedirectToAction("Details", studentId);
        }
    }
}
