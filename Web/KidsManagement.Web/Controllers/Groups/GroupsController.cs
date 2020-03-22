using KidsManagement.Services.Groups;
using KidsManagement.ViewModels.Groups;
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

        public GroupsController(IGroupsService groupsService)
        {
            this.groupsService = groupsService;
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
            var model = this.groupsService.FindById(groupId);

            return await Task.Run(() => View());
        }
    }
}
