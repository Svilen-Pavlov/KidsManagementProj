using KidsManagement.Services.Groups;
using KidsManagement.Services.Levels;
using KidsManagement.Services.Students;
using KidsManagement.Services.Teachers;
using KidsManagement.ViewModels.Groups;
using KidsManagement.ViewModels.Students;
using KidsManagement.ViewModels.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KidsManagement.Web.Controllers.Groups
{
    public class GroupsController : Controller
    {
        private readonly IGroupsService groupsService;
        private readonly ITeachersService teachersService;
        private readonly ILevelsService levelsService;
        private readonly IStudentsService studentsService;

        public GroupsController(IGroupsService groupsService, ITeachersService teachersService, ILevelsService levelsService, IStudentsService studentsService)
        {
            this.groupsService = groupsService;
            this.teachersService = teachersService;
            this.levelsService = levelsService;
            this.studentsService = studentsService;
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
            var model = new CreateEditGroupInputModel()
            {
                Teachers = teachersList,
                Levels = levelsList
            };
            return this.View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateEditGroupInputModel model)
        {
            if (ModelState.IsValid == false)
            {
                model.Teachers=this.teachersService.GetAllForSelection();
                model.Levels = this.levelsService.GetAllForSelection();
                return await Task.Run(() => this.View(model));
            }
            var groupId = await this.groupsService.CreateGroup(model);
            return RedirectToAction("Details", new { groupId = groupId });
        }

        public async Task<IActionResult> Details(int groupId)
        {

            await CheckGroupId(groupId);
            var model = this.groupsService.FindById(groupId); //todo ASYNC

            this.TempData["groupId"] = groupId;
            this.TempData["freeStudentSlots"] = model.MaxStudents - model.Students.Count();
            this.ViewData["freeStudentSlots"] = model.MaxStudents - model.Students.Count();

            return await Task.Run(() => View(model));
        }

        public async Task<IActionResult> UnassignStudent(int studentId)
        {
            await CheckStudentId(studentId);
            int groupId=await CheckGroupId(this.TempData["groupId"]);
            await this.groupsService.RemoveStudentFromGroup(studentId);

            return await Task.Run(() => this.RedirectToAction("Details", new { groupId = groupId }));
        }

        public async Task<IActionResult> ListElligibleStudents()
        {
            int groupId = await CheckGroupId(this.TempData["groupId"]);

            var model = await this.studentsService.GetElligibleGrouplessStudents(groupId);
            this.ViewData["freeStudentSlots"] = this.TempData["freeStudentSlots"];
            this.ViewData["groupId"] = this.TempData["groupId"];
            this.TempData.Keep("freeStudentSlots");
            this.TempData.Keep("groupId");


            return await Task.Run(() => View(model));
        }

        
        public async Task<IActionResult> AddStudents(int studentId)
        {
            await CheckStudentId(studentId);
            int groupId = await CheckGroupId(this.TempData["groupId"]);

            
            this.TempData.Keep("groupId");

            var groupIsFull = await this.groupsService.AddStudentToGroup(groupId, studentId);
            if (groupIsFull ==false)
            {
                this.ViewData["freeStudentSlots"] = (int)this.TempData["freeStudentSlots"]-1;
                this.TempData["freeStudentSlots"] = (int)this.TempData["freeStudentSlots"]-1;
                return await Task.Run(() => this.RedirectToAction("ListElligibleStudents"));
            }
            else
            {
            return await Task.Run(() => this.RedirectToAction("Details", new { groupId = groupId }));
            }

        }

        public async Task<IActionResult> UnassignTeacher(int groupId)
        {
            await CheckGroupId(this.TempData["groupId"]);

            await this.teachersService.UnassignGroup(groupId);

            return await Task.Run(() => this.RedirectToAction("Details", new { groupId = groupId }));
        }
        public async Task<IActionResult> ListFreeTeachers()
        {
            int groupId=await CheckGroupId(this.TempData["groupId"]);
            var model = await this.teachersService.GetAllEligibleTeacherForGroup(groupId);
            this.TempData["groupId"] = groupId;

            return await Task.Run(() => View(model));
        }
        public async Task<IActionResult> Delete(int groupId)
        {
            await CheckGroupId(groupId);

            await this.teachersService.UnassignGroup(groupId);
            var result = await this.groupsService.Delete(groupId);

            return await Task.Run(() => this.RedirectToAction("Index"));
        }

        public async Task<IActionResult> EditInfo(int groupId)
        {
            await CheckGroupId(groupId);
            var model = await this.groupsService.GetInfoForEdit(groupId);
            this.TempData["groupId"] = groupId;

            return await Task.Run(() => this.View(model));
        }

        [HttpPost]
        public async Task<IActionResult> EditInfo(CreateEditGroupInputModel model)
        {
            if (ModelState.IsValid == false) return await Task.Run(() => this.View(model));

            int groupId = await CheckStudentId(TempData["groupId"]);
            model.Id = groupId;
            await this.groupsService.EditInfo(model);

            return await Task.Run(() => this.RedirectToAction("Details", new { groupId = model.Id }));
        }

        public async Task<IActionResult> AssignTeacher(int teacherId)
        {
            int groupId = await CheckGroupId(this.TempData["groupId"]);
            await CheckTeacherId(teacherId);

            await this.teachersService.AssignGroup(teacherId, groupId);

            return await Task.Run(() => this.RedirectToAction("Details", new { groupId = groupId }));
        }
        public async Task<int> CheckGroupId(object groupIdNullable)
        {
            if (groupIdNullable == null || (groupIdNullable is int) == false)
                throw new Exception(); //todo invalid userId Exception

            int groupId = (int)groupIdNullable;
            if (await this.groupsService.GroupExists(groupId) == false)
                throw new Exception(); //todo teacher does not exist Exception

            return groupId;
        }
        public async Task<int> CheckTeacherId(object teacherIdnullabe)
        {
            if (teacherIdnullabe == null || (teacherIdnullabe is int) == false)
                throw new Exception(); //todo invalid userId Exception

            int teacherId = (int)teacherIdnullabe;
            if (await this.teachersService.TeacherExists(teacherId) == false)
                throw new Exception(); //todo teacher does not exist Exception

            return teacherId;
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
    }
}
