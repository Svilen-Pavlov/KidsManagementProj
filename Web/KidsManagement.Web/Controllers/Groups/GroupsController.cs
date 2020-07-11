using KidsManagement.Services.Groups;
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

        public GroupsController(IGroupsService groupsService, ITeachersService teachersService)
        {
            this.groupsService = groupsService;
            this.teachersService = teachersService;
        }
        public async Task<IActionResult> Index()
        {
            var model = this.groupsService.GetAll();

            return await Task.Run(() => View(model)); ; 
        }

        public async Task<IActionResult> Details(int groupId)
        {
            //todo: correct redirect
            if (await this.groupsService.GroupExists(groupId) ==false)
            {
                return this.Redirect("/");
            }
            var model = this.groupsService.FindById(groupId); //todo ASYNC

            return await Task.Run(() => View(model));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var teachersList = this.teachersService.GetAllDropDown();
           // var levelsList = this.levelService.GetAllDropDown();
            var model = new GroupCreateInputModel()
            {
                Teachers = teachersList
            };
            return this.View(model);
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Create(GroupCreateInputModel model)
        {
            if (this.ModelState.IsValid==false)
            {
                return this.Redirect("/"); //todo error page
            }
            var groupId = await this.groupsService.CreateGroup(model);  //todo ASYNC

            return await Task.Run(() => this.Details(groupId));
        }
    }
}
