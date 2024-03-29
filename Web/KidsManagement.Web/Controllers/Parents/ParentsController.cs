﻿using KidsManagement.Services.Groups;
using KidsManagement.Services.Parents;
using KidsManagement.Services.Students;
using KidsManagement.Services.Teachers;
using KidsManagement.ViewModels.Parents;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KidsManagement.Web.Controllers.Parents
{
    public class ParentsController : BaseController
    {
        public ParentsController(IGroupsService groupsService, ITeachersService teachersService, IStudentsService studentsService, IParentsService parentsService)
            : base(groupsService, teachersService, studentsService, parentsService) { }

        public async Task<IActionResult> Index()
        {
            var model = this.parentsService.GetAll();

            return await Task.Run(() => this.View(model));
        }

        public async Task<IActionResult> Create()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEditParentInputModel model)
        {
            if (ModelState.IsValid == false) return await Task.Run(() => this.View());

            var userAdminId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); // from https://stackoverflow.com/questions/30701006/how-to-get-the-current-logged-in-user-id-in-asp-net-core
            var parentId = await this.parentsService.CreateParent(model, userAdminId);

            return await Task.Run(() => RedirectToAction("Details", new { parentId = parentId }));
        }

        public async Task<IActionResult> Details(int parentId)
        {
            await this.CheckParentId(parentId);
            this.TempData["parentId"] = parentId;

            var parent = await this.parentsService.FindById(parentId);
            return await Task.Run(() => this.View(parent));
        }
        public async Task<IActionResult> ListAddStudents() //TODO
        {
            await this.CheckParentId(this.TempData["parentId"]);
            this.TempData.Keep("parentId");

            var students = this.studentsService.GetAll(); //get all students
            return await Task.Run(() => this.View(students));
        }

        public async Task<IActionResult> AddStudents(int studentId) //TODO
        {
            await this.CheckStudentId(studentId);
            await this.CheckParentId(this.TempData["parentId"]);

            this.TempData.Keep("parentId");
            //var groupIsFull = await this.groupsService.AddStudentToGroup(groupId, studentId);
            //if (groupIsFull == false)
            //{
            this.ViewData["freeStudentSlots"] = (int)this.TempData["freeStudentSlots"] - 1;
            this.TempData["freeStudentSlots"] = (int)this.TempData["freeStudentSlots"] - 1; //todo how many children parent currently has
            return await Task.Run(() => this.RedirectToAction("ListElligibleStudents"));
            //}
            //else
            //{
            //    return await Task.Run(() => this.RedirectToAction("Details", new { groupId = groupId }));
            //}
        }

        public async Task<IActionResult> Delete(int parentId)
        {
            await CheckParentId(parentId);

            var result = await this.parentsService.Delete(parentId);

            if (result == 0)
                return await Task.Run(() => this.RedirectToAction("ShowNonQuitStudents", new { parentId = parentId }));
            else
                return await Task.Run(() => this.RedirectToAction("Index"));
        }

        public async Task<IActionResult> ShowNonQuitStudents(int parentId)
        {
            await CheckParentId(parentId);

            var model = await this.parentsService.GetNonQuitStudents(parentId);

            return await Task.Run(() => this.View(model));
        }

        public async Task<IActionResult> EditInfo(int parentId)
        {
            await CheckParentId(parentId);

            var model = await this.parentsService.GetInfoForEdit(parentId);
            this.TempData["parentId"] = parentId;
            this.TempData["profilePicUri"] = model.ProfilePicURI;

            return await Task.Run(() => this.View(model));
        }

        [HttpPost]
        public async Task<IActionResult> EditInfo(CreateEditParentInputModel model)
        {
            int parentId = await CheckParentId(this.TempData["parentId"]);

            if (this.TempData["profilePicUri"] != null)
            {
                model.ProfilePicURI = this.TempData["profilePicUri"].ToString();
                this.TempData.Keep("profilePicUri");
            }

            if (ModelState.IsValid == false) return await Task.Run(() => this.View(model));

            model.Id = parentId;
            await this.parentsService.EditInfo(model);

            return await Task.Run(() => this.RedirectToAction("Details", new { parentId = model.Id }));
        }

        //public async Task<IActionResult> AddNote(int parentId)
        //{
        //    return await Task.Run(() => this.View());
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddNote(int parentId)
        //{

        //}
    }
}
