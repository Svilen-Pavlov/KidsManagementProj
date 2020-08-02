using KidsManagement.Services.Groups;
using KidsManagement.Services.Levels;
using KidsManagement.Services.Teachers;
using KidsManagement.ViewModels.Groups;
using KidsManagement.ViewModels.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KidsManagement.Web.Controllers.Groups
{
    public class GroupsController:Controller
    {
        private readonly IGroupsService groupsService;
        private readonly ITeachersService teachersService;
        private readonly ILevelsService levelsService;

        public GroupsController(IGroupsService groupsService, ITeachersService teachersService, ILevelsService levelsService)
        {
            this.groupsService = groupsService;
            this.teachersService = teachersService;
            this.levelsService = levelsService;
        }
        public async Task<IActionResult> Index()
        {
            var model = this.groupsService.GetAll();

            return await Task.Run(() => View(model)); ; 
        }

       

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var teachersList = this.teachersService.GetAllForSelection();
            var levelsList = this.levelsService.GetAllForSelection();
            var model = new CreateGroupInputModel()
            {
                Teachers = teachersList,
                Levels = levelsList
            };
            return this.View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateGroupInputModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.Redirect("/"); //todo error page
            }
            var groupId = await this.groupsService.CreateGroup(model);  //todo ASYNC
            return RedirectToAction("Details", new { groupId = groupId });
        }

        public async Task<IActionResult> Details(int groupId)
        {
            //todo: correct redirect
            if (await this.groupsService.GroupExists(groupId) == false)
            {
                return this.Redirect("/");
            }
            var model = this.groupsService.FindById(groupId); //todo ASYNC

            return await Task.Run(() => View(model));
        }
    }
}
