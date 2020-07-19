using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Constants;
using KidsManagement.Services.External.CloudinaryService;
using KidsManagement.ViewModels.Parents;
using KidsManagement.ViewModels.Students;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services.Students
{
    public class StudentsService : IStudentsService
    {
        private readonly KidsManagementDbContext db;
        private readonly ICloudinaryService cloudinaryService;

        public StudentsService(KidsManagementDbContext db, ICloudinaryService cloudinaryService)
        {
            this.db = db;
            this.cloudinaryService = cloudinaryService;
        }
        public async Task<int> CreateStudent(CreateStudentInputModel model)
        {
            var pic = model.ProfileImage;
            var picURI= pic == null ? string.Empty : await this.cloudinaryService.UploadProfilePicASync(pic);
            var age = DateTime.Today.Year - model.BirthDate.Year;
            if (model.BirthDate.Date > DateTime.Today.AddYears(-age)) age--; //Case for a leap year

           // var parentIds = model.Parents.Where(x => x.Selected).Select(x => x.Id).ToArray();
           // var parentsForStudent = this.db.Parents.Where(x => parentIds.Contains(x.Id)).ToArray();

            var student = new Student
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Gender = model.Gender,
                Age = age,
                BirthDate = model.BirthDate,
                Grade = model.Grade,
                Status = model.Status,
                //Parents= parentsForStudent.Select(p => new StudentParent { Parent = p }).ToArray(),
            };

            await this.db.Students.AddAsync(student);
            await this.db.SaveChangesAsync();
            return student.Id;
        }

        public async Task AssignStudentToGroup(int studentId, int groupId)
        {
            var student=await this.db.Students.FirstOrDefaultAsync(x => x.Id == studentId);

            student.GroupId = groupId;

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> StudentExists(int StudentId) // with or w/o deleted?
        {
            return await this.db.Students.AnyAsync(x => x.Id == StudentId);
        }

        public async Task<StudentDetailsViewModel> FindById(int studentId)
        {
            var student = await this.db.Students.FirstOrDefaultAsync(x => x.Id == studentId);

            var model = new StudentDetailsViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                Age = student.Age,
                BirthDate = student.BirthDate.ToString(Const.dateOnlyFormat),
                Gender = student.Gender,
                Grade = student.Grade,
                GroupId = (int?)student.GroupId == null ? 0 : student.GroupId,
                GroupName = student.Group == null ? InfoStrings.StudentNotInAGroupYet : student.Group.Name,
                Status = student.Status,
                ProfilePicURI=student.ProfilePicURI,
                Parents=student.Parents.Select(p=> new ParentsSelectionViewModel
                {
                    Id=p.ParentId,
                    Name=p.Parent.FullName
                }).ToList()
            };

            return model;
        }

        public AllStudentsDetailsViewModel GetAll()
        {
            var students = this.db.Students
               .Select(student => new AllSingleStudentsViewModel
               {
                   Id = student.Id,
                   FulLName = student.FullName,
                   Gender = student.Gender,
                   Age = student.Age,
                   GroupName = student.Group == null ? "Not in a group yet" : student.Group.Name,
               })
               .ToArray()
               .OrderBy(x => x.FulLName)
                .ToArray();


            var studentsList = new List<AllSingleStudentsViewModel>(students);

            var model = new AllStudentsDetailsViewModel() {  Students= studentsList };

            return model;
        }

        public async Task EditParents(EditParentsInputModel model)
        {
            var student = this.db.Students.FirstOrDefaultAsync(x => x.Id == model.StudentId).Result;

            var parentIds = model.Parents.Where(x => x.Selected).Select(x => x.Id).ToArray();
            var parentsForStudent = this.db.Parents.Where(x => parentIds.Contains(x.Id)).ToArray();

            foreach (var parent in parentsForStudent)
            {
                var link = new StudentParent { ParentId = parent.Id };
                //var link = new StudentParent { ParentId = parent.Id, StudentId=student.Id };
                student.Parents.Add(link);
            }

            await this.db.SaveChangesAsync();

        }
    }
}
