using KidsManagement.Services.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KidsManagement.Web.Areas.Administration.Admin
{
    [Authorize(Roles ="Admin")]
    public class AdminController:Controller
    {
        private readonly ITeachersService teachersService;

        public AdminController(ITeachersService teachersService)
        {
            this.teachersService = teachersService;
        }
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult TeachersList()
        {
            var model=this.teachersService.GetAll();
            return this.View(model);
        }


    }
}
