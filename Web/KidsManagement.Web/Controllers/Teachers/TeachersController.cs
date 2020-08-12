using KidsManagement.Data.Models.Enums;
using KidsManagement.Services.Groups;
using KidsManagement.Services.Levels;
using KidsManagement.Services.Students;
using KidsManagement.Services.Teachers;
using KidsManagement.ViewModels.Groups;
using KidsManagement.ViewModels.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KidsManagement.Web.Controllers.Teachers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class TeachersController : Controller
    {
        private readonly ITeachersService teachersService;
        private readonly ILevelsService levelsService;
        private readonly IGroupsService groupsService;
        private readonly IStudentsService studentsService;

        public TeachersController(ITeachersService teachersService, ILevelsService levelsService, IGroupsService groupsService, IStudentsService studentsService)
        {
            this.groupsService = groupsService;
            this.studentsService = studentsService;
            this.teachersService = teachersService;
            this.levelsService = levelsService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var levelsList = this.levelsService.GetAllForSelection();
            var groupsList = this.groupsService.GetAllForSelection(false); //the int? argument is when wanting to add only empty groups to the teacher or other teacher's groups
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

            if (await this.teachersService.UserExists(model.Username))
            {
                return this.Redirect("/"); //todo error page User Exists
            }

            var newTeacherId = await this.teachersService.CreateTeacher(model);  //todo ASYNC
            return RedirectToAction("Details", new { teacherId = newTeacherId });
        }

        public async Task<IActionResult> Details(int teacherId)
        {
            
            if (await this.teachersService.TeacherExists(teacherId) == false)
            {
                return this.Redirect("/"); //todo: error page
            }
            var model = this.teachersService.FindById(teacherId); //todo ASYNC

            return await Task.Run(() => View(model));
        }

        public async Task<IActionResult> AddGroups(int teacherId)
        {
            if (await this.teachersService.TeacherExists(teacherId)==false)
            {
                return this.Redirect("/"); //invalid teacher ERROR
            }
            this.TempData["teacherId"] = teacherId;
            var groupsList = this.groupsService.GetAllForSelection(false).ToList();

            var outputModel = new AddGroupsToTeacherViewModel() { Groups = groupsList};
            return await Task.Run(() => this.View("AddGroups", outputModel));
        }

        [HttpPost]
        public async Task<IActionResult> AddGroups(AddGroupsToTeacherViewModel model)
        {
            var teacherId = this.TempData["teacherId"];
            if (teacherId == null || (teacherId is int) == false)
                return this.Redirect("/"); //invalid teacher ERROR
            
            model.TeacherId = (int)teacherId;

            if (await this.teachersService.TeacherExists(model.TeacherId) == false)
            {
                return this.Redirect("/"); //invalid teacher ERROR
            }

            await this.teachersService.AddGroups(model);

            return await Task.Run(()=>RedirectToAction("Details", new { teacherId = model.TeacherId }));
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> MyZone()
        {
            var userTeachernId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var teacherId = this.teachersService.GetBussinessIdByUserId(userTeachernId).Result;
            this.TempData["teacherId"] = teacherId;

            return await Task.Run(() => this.View());
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> MyStudents()
        {
            var teacherId = TempData["teacherId"];
            this.TempData.Keep("teacherId");

            if (teacherId == null || (teacherId is int) == false)
                return this.Redirect("/"); //invalid teacher ERROR
            var teacherIdInt = (int)teacherId;



            var viewModel = this.studentsService.GetAll(teacherIdInt);

            return await Task.Run(() => this.View(viewModel));
        }


    }
}
