using KidsManagement.Data;
using KidsManagement.Data.Models;
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

        public StudentsService(KidsManagementDbContext db)
        {
            this.db = db;
        }
        public async Task<int> CreateStudent(CreateStudentInputModel model)
        {
            //how to fill parents
            var student = new Student
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Gender = model.Gender,
                Age = model.Age,
                BirthDate = model.BirthDate,
                Grade = model.Grade,
                Status = model.Status,
                CreatedOn = DateTime.UtcNow

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
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                Age = student.Age,
                BirthDate = student.BirthDate,
                Gender = student.Gender,
                Grade = student.Grade,
                GroupId = (int)student.GroupId,
                Status = student.Status
            };

            return model;
        }
    }
}
