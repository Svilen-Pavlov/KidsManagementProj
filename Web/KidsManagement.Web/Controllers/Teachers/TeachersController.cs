using KidsManagement.Data.Models.Enums;
using KidsManagement.Services.Groups;
using KidsManagement.Services.Levels;
using KidsManagement.Services.Teachers;
using KidsManagement.ViewModels.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KidsManagement.Web.Controllers.Teachers
{
    [Authorize(Roles = "Admin")]
    public class TeachersController : Controller
    {
        private readonly ITeachersService teachersService;
        private readonly ILevelsService levelsService;
        private readonly IGroupsService groupsService;



        public TeachersController(ITeachersService teachersService, ILevelsService levelsService, IGroupsService groupsService)
        {
            this.groupsService = groupsService;
            this.teachersService = teachersService;
            this.levelsService = levelsService;
        }



        public IActionResult Create()
        {
            var levelsList = this.levelsService.GetAllForSelection();
            var groupsList = this.groupsService.GetAllForSelection(null); //this is when wanting to add only empty groups to the teacher
            var model = new CreateTeacherInputModel()
            {
                Levels = levelsList.ToList(),
                Groups = groupsList.ToList()
            };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeacherInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.Redirect("/"); //todo error page
            }
            var teacherId = await this.teachersService.CreateTeacher(model);  //todo ASYNC
            return RedirectToAction("Details", new { groupId = teacherId });
        }

        public async Task<IActionResult> Details(int teacherId)
        {
            //todo: correct redirect
            if (await this.teachersService.TeacherExists(teacherId) == false)
            {
                return this.Redirect("/");
            }
            var model = this.teachersService.FindById(teacherId); //todo ASYNC

            return await Task.Run(() => View(model));
        }
    }
}
