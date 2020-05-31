﻿using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.Services.External.CloudinaryService;
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
            //var pic = model.ProfilePic;
            //var picURI=await this.cloudinaryService.UploadProfilePicASync(pic);

            //how to fill parents

            var age = DateTime.Today.Year - model.BirthDate.Year;
            if (model.BirthDate.Date > DateTime.Today.AddYears(-age)) age--; // Go back to the year the person was born in case of a leap year


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
                Id=student.Id,
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                Age = student.Age,
                BirthDate = student.BirthDate,
                Gender = student.Gender,
                Grade = student.Grade,
                GroupId = (int)student.GroupId,
                Status = student.Status,
                ProfilePicURI=student.ProfilePicURI
            };

            return model;
        }

        public AllStudentsDetailsViewModel GetAll()
        {
            var students = this.db.Students
               .Select(x => new AllSingleStudentsViewModel
               {
                   Id = x.Id,
                   FulLName = x.FullName,
                   Gender = x.Gender,
                   Age = x.Age,
                   GroupName = x.Group.Name,
               })
               .ToArray()
               .OrderBy(x => x.FulLName);


            var studentsList = new List<AllSingleStudentsViewModel>(students);

            var model = new AllStudentsDetailsViewModel() {  Students= studentsList };

            return model;
        }
    }
}
