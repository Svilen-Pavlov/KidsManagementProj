using KidsManagement.Services.External.CloudinaryService;
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

        public StudentsController(IStudentsService studentsService, ICloudinaryService cloudinaryService, IParentsService parentsService)
        {
            this.studentsService = studentsService;
            this.cloudinaryService = cloudinaryService;
            this.parentsService = parentsService;
        }

        public async Task<IActionResult> Index()
        {
            var model = this.studentsService.GetAll();
            return await Task.Run(() => this.View(model));
        }


        public async Task<IActionResult> Create()
        {
            //var model = new CreateStudentInputModel();
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
            var parents = this.parentsService.GetAllForSelection(studentId).ToList();
            var outputModel = new EditParentsInputModel() { StudentId = studentId, Parents=parents };
           //to use ViewMabg instead of ID
            return await Task.Run(()=>this.View("EditParents", outputModel));
        }

        //public async Task<IActionResult> EditStudentParents(EditParentsInputModel model)
        //{
        //    var outputModel = new EditParentsInputModel() { StudentId = model.StudentId, Parents = model.Parents };

        //    return await Task.Run(() => View(outputModel));
        //}

        [HttpPost]
        public async Task<IActionResult> EditStudentParents(EditParentsInputModel model)
        {
            await this.studentsService.EditParents(model);

            return await Task.Run(() => this.RedirectToAction("Details", new { studentId = model.StudentId }));
        }






        public async Task<IActionResult> Details(int studentId)
        {
            if (await this.studentsService.StudentExists(studentId) == false)
            {
                this.Redirect("/");
            }

            var studentModel = await this.studentsService.FindById(studentId);
            return this.View(studentModel);
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
