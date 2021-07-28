using KidsManagement.Services.Groups;
using KidsManagement.Services.Levels;
using KidsManagement.Services.Parents;
using KidsManagement.Services.Students;
using KidsManagement.Services.Teachers;
using KidsManagement.ViewModels.Groups;
using KidsManagement.ViewModels.Levels;
using KidsManagement.ViewModels.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KidsManagement.Web.Controllers.Teachers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class TeachersController : BaseController
    {
        private readonly ILevelsService levelsService;

        public TeachersController(ITeachersService teachersService, ILevelsService levelsService, IGroupsService groupsService, IStudentsService studentsService, IParentsService parentsService)
            :base(groupsService,teachersService,studentsService,parentsService)
        {
            this.levelsService = levelsService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var levelsList = this.levelsService.GetAllForSelection();
            var groupsList = this.groupsService.GetAllForSelection(false); //the int? argument is for adding only empty groups to the teacher or other teacher's groups
            var model = new CreateEditTeacherInputModel()
            {
                Levels = levelsList.ToList(),
                Groups = groupsList.ToList()
            };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEditTeacherInputModel model)
        {
            if (ModelState.IsValid == false)
            {
                model.Levels = this.levelsService.GetAllForSelection().ToList();
                model.Groups = this.groupsService.GetAllForSelection(false).ToList();
                return await Task.Run(() => this.View(model));
            }

            if (await this.teachersService.UserExists(model.Username)) { return this.Json("Existing Username"); }  //todo error page User Exists and in a separate method

            var newTeacherId = await this.teachersService.CreateTeacher(model);  
            return RedirectToAction("Details", new { teacherId = newTeacherId });
        }

        public async Task<IActionResult> Details(int teacherId)
        {
            await CheckTeacherId(teacherId);

            this.TempData["teacherId"] = teacherId;
            var model = this.teachersService.FindById(teacherId);

            return await Task.Run(() => View(model));
        }

        public async Task<IActionResult> AddGroups(int teacherId)
        {
            await CheckTeacherId(teacherId);
            
            this.TempData["teacherId"] = teacherId;
            var groupsList = this.groupsService.GetAllForSelection(false).ToList();
            var outputModel = new AddGroupsToTeacherViewModel() { Groups = groupsList};

            return await Task.Run(() => this.View("AddGroups", outputModel));
        }

        [HttpPost]
        public async Task<IActionResult> AddGroups(AddGroupsToTeacherViewModel model)
        {
            var teacherId = await CheckTeacherId(this.TempData["teacherId"]);

            await this.teachersService.AddGroups(model);

            return await Task.Run(()=>RedirectToAction("Details", new { teacherId = teacherId }));
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> MyZone() //todo input teacherId for Admin to be able to check teacher Zone
        {
            var teacherIdnullable = await GetLoggedInTeacherBussinessId();
            var teacherId=await CheckTeacherId(teacherIdnullable); 

            var model = this.teachersService.GetMyZoneInfo(teacherId);

            return await Task.Run(() => this.View(model));
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> MyStudents()
        {
            var teacherIdnullable = await GetLoggedInTeacherBussinessId();
            var teacherId = await CheckTeacherId(teacherIdnullable);

            var viewModel = this.studentsService.GetAll(teacherId);

            return await Task.Run(() => this.View(viewModel));
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> MyGroups()
        {
            var teacherIdnullable = await GetLoggedInTeacherBussinessId();
            var teacherId = await CheckTeacherId(teacherIdnullable);

            var viewModel = this.groupsService.GetAll(teacherId);

            return await Task.Run(() => this.View(viewModel));
        }

        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> MySchedule(int marker)
        {
            var teacherIdnullable = await GetLoggedInTeacherBussinessId();
            var teacherId = await CheckTeacherId(teacherIdnullable);
           
            if (marker == 0)
                this.TempData["date"] = DateTime.Now;

            DateTime date = DateTime.Parse(this.TempData["date"].ToString());

            var viewModel = this.teachersService.GetMySchedule(teacherId, date, marker);

            this.TempData["date"] = viewModel.FromDate;

            return await Task.Run(() => this.View(viewModel));
        }

        public async Task<IActionResult> EditInfo(int teacherId)
        {
            await CheckTeacherId(teacherId);
            var model = await this.teachersService.GetInfoForEdit(teacherId);
            this.TempData["teacherId"] = teacherId;
            this.TempData["profilePicUri"] = model.ProfilePicURI;

            return await Task.Run(() => this.View(model));
        }

        [HttpPost]
        public async Task<IActionResult> EditInfo(CreateEditTeacherInputModel model)
        {
            model.ProfilePicURI = this.TempData["profilePicUri"].ToString();
            this.TempData.Keep("profilePicUri");

            if (ModelState.IsValid == false) return await Task.Run(() => this.View(model));

            int teacherId = await CheckTeacherId(this.TempData["teacherId"]);

            model.Id = teacherId;
            await this.teachersService.EditInfo(model);

            return await Task.Run(() => this.RedirectToAction("Details", new { teacherId = model.Id }));
        }

        public async Task<IActionResult> EditLevels(int teacherId)
        {
            await CheckTeacherId(teacherId);
            var model = this.levelsService.GetLevelsForEdit(teacherId);
           
            this.TempData["teacherId"] = teacherId;

            return await Task.Run(() => this.View(model));
        }

        [HttpPost]
        public async Task<IActionResult> EditLevels(EditTeacherLevelsViewModel model)
        {
            int teacherId = await CheckTeacherId(this.TempData["teacherId"]);

            model.TeacherId = teacherId;
            await this.levelsService.EditLevelsOfTeacher(model);

            return await Task.Run(() => this.RedirectToAction("Details", new { teacherId = model.TeacherId }));
        }
        
        public async Task<IActionResult> UnassignGroup(int groupId)
        {
            int teacherId = await CheckTeacherId(this.TempData["teacherId"]);
            await CheckGroupId(groupId);

            var result = await this.teachersService.UnassignGroup(groupId);

            return await Task.Run(() => this.RedirectToAction("Details", new { teacherId = teacherId }));
        }

        public async Task<IActionResult> Delete(int teacherId)
        {
            await CheckTeacherId(teacherId);

            var result = await this.teachersService.Delete(teacherId);

            if (result == 0)
                return await Task.Run(() => this.RedirectToAction("RemainingGroups", new { teacherId = teacherId }));
            else
                return await Task.Run(() => this.RedirectToAction("Index"));
        }

        public async Task<IActionResult> RemainingGroups(int teacherId)
        {
            await CheckTeacherId(teacherId);

            var model = this.groupsService.GetActiveGroupsByTeacher(teacherId);
            return await Task.Run(() => this.RedirectToAction("TeachersList", "Admin"));

        }

        public async Task<int?> GetLoggedInTeacherBussinessId()
        {
            var userTeacherId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var teacherId = await this.teachersService.GetBussinessIdByUserId(userTeacherId);
           
            return teacherId;
        }
    }
}
