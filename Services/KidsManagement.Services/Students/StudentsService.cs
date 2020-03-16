using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.ViewModels.Students;
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


        public bool StudentExists(int StudentId) // with or w/o deleted?
        {
            return this.db.Students.Any(x => x.Id == StudentId);
        }
    }
}
