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
           
            if (await this.groupsService.GroupExists(groupId) == false)
            {
                return this.Redirect("/");  //todo: correct redirect
            }
            var model = this.groupsService.FindById(groupId); //todo ASYNC

            this.TempData["groupId"] = groupId;
            //this.TempData["studentStatus"] = model.Status.ToString(); // additional info for the process (how free spots count)

            return await Task.Run(() => View(model));
        }

        //public async Task<IActionResult> AddStudents()
        //{
        //    var groupIdNullable = this.TempData["groupId"];
        //    if (groupIdNullable == null || (groupIdNullable is int) == false)
        //        return this.Redirect("/");

        //    int groupId = (int)groupIdNullable;

        //    if (await this.groupsService.GroupExists(groupId) == false)
        //    {
        //        return this.Redirect("/");
        //    }

        //    this.TempData["groupId"] = groupId;

        //    var studentsList = await this.studentService.GetEligibleGrouplessStudents(groupId);

        //    var model = new AssignStudentsToGroupInputModel() { StudentsForSelection = studentsList.ToList() };
        //    //this.TempData.Keep("studentStatus"); // additional info for the process (how free spots count)
        //    this.TempData.Keep("groupId");

        //    return await Task.Run(() => View(model));
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddStudents(AssignStudentsToGroupInputModel model)
        //{
        //    var groupIdNullable = this.TempData["groupId"];
        //    var studentsList = model.StudentsList; //get a List of Selected Students for addition
        //    if (groupIdNullable == null || (groupIdNullable is int) == false)
        //        return this.Redirect("/"); //todo groupId is null or not int

        //    int groupId = (int)groupIdNullable;


        //    await this.groupsService.AssignStudentsToGroup(groupId, studentsList);

        //    return await Task.Run(() => this.RedirectToAction("Details", new { groupId = groupId }));
        //}
    }
}
