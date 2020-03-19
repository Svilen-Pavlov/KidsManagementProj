using KidsManagement.Services.Students;
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
            return this.Redirect("/");
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
    }
}
