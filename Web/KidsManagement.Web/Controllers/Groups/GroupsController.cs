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
        public IActionResult Index()
        {
            var model = this.groupsService.GetAll();

            return this.View(model); 
        }
    }
}
