using KidsManagement.Services.External.CloudinaryService;
using KidsManagement.Services.Groups;
using KidsManagement.Services.Parents;
using KidsManagement.Services.Students;
using KidsManagement.ViewModels.Parents;
using KidsManagement.ViewModels.Students;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create(CreateStudentInputModel model) //tozi model se vru6ta ot viewto
        {
            if (ModelState.IsValid == false)
            {
                return this.Redirect("/"); //invalid student create ERROR
            }

            var studentId = await this.studentsService.CreateStudent(model);
            this.TempData["studentId"] = studentId;
            var parents = this.parentsService.GetAllForSelection(studentId).ToList();
            var outputModel = new EditParentsInputModel() { Parents = parents };

            return await Task.Run(() => this.View("EditParents", outputModel));
        }



        [HttpPost]
        public async Task<IActionResult> EditStudentParents(EditParentsInputModel model)
        {
            var studentId = this.TempData["studentId"];
            if (studentId == null || (studentId is int) == false)
                return this.Redirect("/"); //invalid student create ERROR

            model.StudentId = (int)studentId;
            await this.studentsService.EditParents(model);

            return await Task.Run(() => this.RedirectToAction("Details", new { studentId = model.StudentId }));
        }

        public async Task<IActionResult> Details(int studentId)
        {
            if (await this.studentsService.Exists(studentId) == false)
            {
                this.Redirect("/");
            }
            var model = await this.studentsService.FindById(studentId);
            
            this.TempData["studentId"] = studentId;
            this.TempData["studentStatus"] = model.Status.ToString();

            return this.View(model);
        }

        public async Task<IActionResult> AddToGroup()
        {
            var studentId = this.TempData["studentId"];
            if (studentId == null || (studentId is int) == false)
                return this.Redirect("/");

            int studentIdInt = (int)studentId;

            if (await this.studentsService.Exists(studentIdInt) == false)
            {
                this.Redirect("/");
            }

            var groupsList = await this.groupsService.GetVacantGroupsWithProperAge(studentIdInt);

            var outputModel = new AddStudentToGroupInputModel() {GroupsForSelection = groupsList.ToList() };
            this.TempData.Keep("studentStatus");
            this.TempData.Keep("studentId");

            return await Task.Run(() => this.View(outputModel));
        }

        [HttpPost]
        public async Task<IActionResult> AddToGroup(AddStudentToGroupInputModel model)
        {
            var studentId = this.TempData["studentId"];
            var groupId = model.IsSelected;
            if (studentId == null || (studentId is int) == false)
                return this.Redirect("/"); //todo groupId is null or not int

            if (await this.groupsService.GroupExists(groupId) == false)
            {
                return this.Redirect("/"); // group does not exist
            }
            int studentIdInt = (int)studentId;

            if (this.TempData["studentStatus"].ToString() == "Active")
               await this.groupsService.RemoveStudent(studentIdInt);

            await this.groupsService.AddStudent(studentIdInt, groupId);

            return await Task.Run(() => this.RedirectToAction("Details", new { studentId = studentId }));
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int studentId)
        {
            if (file == null)
            {
                return this.Redirect("/"); //FILE IS NULL ERROR
            }
            var a = this.RouteData.Values;
            var entityId = studentId;
            var picURI = await this.cloudinaryService.UploadProfilePicASync(file);

            return this.RedirectToAction("Details", entityId);
        }
    }
}
