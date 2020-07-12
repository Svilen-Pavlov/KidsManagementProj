using KidsManagement.Services.Groups;
using KidsManagement.Services.Teachers;
using KidsManagement.ViewModels.Groups;
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
        private readonly IGroupsService groupsService;

        public AdminController(ITeachersService teachersService, IGroupsService groupsService)
        {
            this.teachersService = teachersService;
            this.groupsService = groupsService;
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
        public IActionResult AllGroupsOfTeacher(int teacherId)
        {
            var model = this.groupsService.GetAllByTeacher(teacherId);
            return this.View(model);   
        }

        public IActionResult AddTeacher()
        {
            return this.View();
        }


    }
}
