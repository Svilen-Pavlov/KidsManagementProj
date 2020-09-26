﻿using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
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
            var picURI = pic == null ? string.Empty : await this.cloudinaryService.UploadProfilePicASync(pic);
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
                ProfilePicURI = picURI
            };

            await this.db.Students.AddAsync(student);
            await this.db.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> Exists(int StudentId) // with or w/o deleted?
        {
            return await this.db.Students.AnyAsync(x => x.Id == StudentId && x.Status != StudentStatus.Quit);
        }

        public async Task<StudentDetailsViewModel> FindById(int studentId)
        {
            var student = await this.db.Students
                //.Include(s=>s.Parents)
                //.ThenInclude(x=>x.Select(y=>y.Parent))
                .Include(s => s.Group)
                .FirstOrDefaultAsync(x => x.Id == studentId && x.Status != StudentStatus.Quit);
            var parents = this.db.Parents.Where(p => p.Children.Any(sp => sp.StudentId == student.Id)).ToArray();

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
                ProfilePicURI = student.ProfilePicURI,
                Parents = parents.Select(p => new ParentsSelectionViewModel
                {
                    Id = p.Id,
                    Name = p.FullName
                }).ToList()
            };

            return model;
        }

        public AllStudentsDetailsViewModel GetAll()
        {
            return this.GetAll(0);
        }

        public AllStudentsDetailsViewModel GetAll(int teacherId)
        {
            var students = this.db.Students
                .Where(s => (teacherId != 0) ? s.Group.TeacherId == teacherId : true)
                .Where(s => s.Status != StudentStatus.Quit)
               .Select(student => new AllSingleStudentsViewModel
               {
                   Id = student.Id,
                   FulLName = student.FullName,
                   Gender = student.Gender,
                   Age = student.Age,
                   GroupName = student.Group == null ? InfoStrings.StudentNotInAGroupYet : student.Group.Name,
               })
               .ToArray()
               .OrderBy(x => x.FulLName)
                .ToArray();


            var studentsList = new List<AllSingleStudentsViewModel>(students);

            var model = new AllStudentsDetailsViewModel() { Students = studentsList };

            return model;
        }

        public async Task AddParents(EditParentsInputModel model)
        {
            var student = this.db.Students.FirstOrDefaultAsync(x => x.Id == model.StudentId).Result;

            var parentIds = model.Parents.Where(x => x.Selected).Select(x => x.Id).ToArray();
            var parentsForStudent = this.db.Parents.Where(x => parentIds.Contains(x.Id)).ToArray();

            foreach (var parent in parentsForStudent)
            {
                var link = new StudentParent { ParentId = parent.Id };
                student.Parents.Add(link);
            }

            await this.db.SaveChangesAsync();

        }

        public async Task<int> UnassignParent(int studentId, int parentId)
        {
            var link = await this.db.StudentParents.FirstOrDefaultAsync(sp => sp.StudentId == studentId && sp.ParentId == parentId);

            this.db.StudentParents.Remove(link);

            return await this.db.SaveChangesAsync();
        }

        public async Task<int> Delete(int studentId)
        {
            var student = await this.db.Students
                .Include(s => s.Group)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student.Group != null)
                student.Group.Students.Remove(student);

            student.Status = StudentStatus.Quit;


            return this.db.SaveChangesAsync().Result;
        }

        public async Task<CreateStudentInputModel> GetInfoForEdit(int studentId)
        {
            var student = await this.db.Students
               .FirstOrDefaultAsync(x => x.Id == studentId);

            var model = new CreateStudentInputModel
            {
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                BirthDate = student.BirthDate,
                Gender = student.Gender,
                Grade = student.Grade,
                ProfilePicURI = student.ProfilePicURI,
            };

            return model;
        }

        public async Task EditInfo(CreateStudentInputModel model)
        {
            var pic = model.ProfileImage;
            var picURI = pic == null ? string.Empty : await this.cloudinaryService.UploadProfilePicASync(pic);
            var age = DateTime.Today.Year - model.BirthDate.Year;
           
            if (model.BirthDate.Date > DateTime.Today.AddYears(-age)) age--; //Case for a leap year

            var student = await this.db.Students.FirstOrDefaultAsync(x => x.Id == model.Id);

            student.FirstName = model.FirstName;
            student.MiddleName = model.MiddleName;
            student.LastName = model.LastName;
            student.Gender = model.Gender;
            student.Age = age;
            student.BirthDate = model.BirthDate;
            student.Grade = model.Grade;
            student.ProfilePicURI = picURI;

            await this.db.SaveChangesAsync();
        }
    }
}
