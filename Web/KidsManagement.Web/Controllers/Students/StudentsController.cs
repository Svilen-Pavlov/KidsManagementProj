using KidsManagement.Services.External.CloudinaryService;
using KidsManagement.Services.Groups;
using KidsManagement.Services.Parents;
using KidsManagement.Services.Students;
using KidsManagement.ViewModels.Parents;
using KidsManagement.ViewModels.Students;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KidsManagement.Web.Controllers.Students
{
    public class StudentsController : Controller
    {
        private readonly IStudentsService studentsService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IParentsService parentsService;
        private readonly IGroupsService groupsService;

        public StudentsController(IStudentsService studentsService, ICloudinaryService cloudinaryService, IParentsService parentsService, IGroupsService groupsService)
        {
            this.studentsService = studentsService;
            this.cloudinaryService = cloudinaryService;
            this.parentsService = parentsService;
            this.groupsService = groupsService;
        }

        public async Task<IActionResult> Index()
        {
            var model = this.studentsService.GetAll();
            return await Task.Run(() => this.View(model));
        }


        public async Task<IActionResult> Create()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEditStudentInputModel model)
        {
            if (ModelState.IsValid == false) return this.Redirect("/"); //todo error

            var studentId = await this.studentsService.CreateStudent(model);
            this.TempData["studentId"] = studentId;

            return await Task.Run(() => this.RedirectToAction("Details", new { studentId = studentId }));
        }


        public async Task<IActionResult> Details(int studentId)
        {
            await CheckStudentId(studentId);

            var model = await this.studentsService.FindById(studentId);

            this.TempData["studentId"] = studentId;
            this.TempData["studentStatus"] = model.Status.ToString();

            return await Task.Run(() => this.View(model));
        }



        public async Task<IActionResult> AddParents()
        {
            int studentId = await CheckStudentId(this.TempData["studentId"]);

            var parents = this.parentsService.GetAllForSelection(studentId).ToList();
            var outputModel = new EditParentsInputModel() { Parents = parents };
            this.TempData.Keep("studentId");

            return await Task.Run(() => this.View("AddParents", outputModel));
        }

        [HttpPost]
        public async Task<IActionResult> AddParents(EditParentsInputModel model)
        {
            model.StudentId = await CheckStudentId(this.TempData["studentId"]);
            await this.studentsService.AddParents(model);

            return await Task.Run(() => this.RedirectToAction("Details", new { studentId = model.StudentId }));
        }

        public async Task<IActionResult> UnassignParent(int parentId)
        {
            int studentId = await CheckStudentId(this.TempData["studentId"]);
            await CheckParentId(parentId);
            var result = await this.studentsService.UnassignParent(studentId, parentId);

            return await Task.Run(() => this.RedirectToAction("Details", new { studentId = studentId }));
        }

        public async Task<IActionResult> AddToGroup()
        {
            int studentId = await CheckStudentId(this.TempData["studentId"]);

            var groupsList = await this.groupsService.GetVacantGroupsWithProperAge(studentId);

            var model = new AddStudentToGroupInputModel() { GroupsForSelection = groupsList.ToList() };
            this.TempData.Keep("studentStatus");
            this.TempData.Keep("studentId");

            return await Task.Run(() => View(model));
        }

        [HttpPost]
        public async Task<IActionResult> AddToGroup(AddStudentToGroupInputModel model)
        {
            int studentId = await CheckStudentId(this.TempData["studentId"]);
            int groupId = model.IsSelected;

            if (this.TempData["studentStatus"].ToString() == "Active")
                await this.groupsService.RemoveStudent(studentId);

            await this.groupsService.AddStudent(studentId, groupId);

            return await Task.Run(() => this.RedirectToAction("Details", new { studentId = studentId }));
        }

        public async Task<IActionResult> Delete(int studentId)
        {
            await CheckStudentId(studentId);
            var result = await this.studentsService.Delete(studentId);

            return await Task.Run(() => this.RedirectToAction("Index"));
        }

        public async Task<IActionResult> EditInfo(int studentId)
        {
            await CheckStudentId(studentId);
            var model = await this.studentsService.GetInfoForEdit(studentId);
            this.TempData["studentId"] = studentId;


            return await Task.Run(() => this.View(model));
        }

        [HttpPost]
        public async Task<IActionResult> EditInfo(CreateEditStudentInputModel model)
        {
            ////a  mix of edit/create/details
            int studentId=await CheckStudentId(TempData["studentId"]);
            model.Id = studentId;
            await this.studentsService.EditInfo(model);

            return await Task.Run(() => this.RedirectToAction("Details", new { studentId = model.Id }));
        }

        public async Task<int> CheckStudentId(object studentIdNullable)
        {
            if (studentIdNullable == null || (studentIdNullable is int) == false)
                throw new Exception(); //todo invalid userId Exception

            int studentId = (int)studentIdNullable;
            if (await this.studentsService.Exists(studentId) == false)
                throw new Exception(); //todo student does not exist Exception

            return studentId;
        }

        public async Task<int> CheckParentId(object parentIdNullable)
        {
            if (parentIdNullable == null || (parentIdNullable is int) == false)
                throw new Exception(); //todo invalid userId Exception

            int parentId = (int)parentIdNullable;

            if (await this.parentsService.Exists(parentId) == false)
                throw new Exception(); //todo parent does not exist Exception

            return parentId;
        }

    }
}
