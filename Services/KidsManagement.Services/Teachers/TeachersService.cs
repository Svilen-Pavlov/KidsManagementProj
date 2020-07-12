using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.ViewModels.Groups;
using KidsManagement.ViewModels.Teachers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services.Teachers
{
    public class TeachersService : ITeachersService
    {
        private readonly KidsManagementDbContext db;

        public TeachersService(KidsManagementDbContext db)
        {
            this.db = db;
        }

        public async Task<int> CreateTeacher(TeacherCreateInputModel model) //idk if this populates teacherlevels correctly
        {
          
            var teacher = new Teacher
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                HiringDate = model.HiringDate,
                DismissalDate = model.DismissalDate,
                Salary = model.Salary,
            };
            await this.db.Teachers.AddAsync(teacher);

            var levelIds = model.Levels.Select(x => x.Id).ToArray();
            var levelTeacherList = new List<LevelTeacher>();
            foreach (var levelId in levelIds)
            {
                var levelTeacher = new LevelTeacher
                {
                    LevelId = levelId,
                    TeacherId = teacher.Id
                };
                levelTeacherList.Add(levelTeacher);

            }

            await this.db.LevelTeachers.AddRangeAsync(levelTeacherList);
            await this.db.SaveChangesAsync();

            return teacher.Id;
        }


        public TeacherDetailsViewModel FindById(int teacherId)
        {
            var teacher = this.db.Teachers.FirstOrDefault(x => x.Id == teacherId);
            var levelsIds = this.db.LevelTeachers.Where(x => x.TeacherId == teacherId).Select(x => x.LevelId).ToArray();
            var levels = this.db.Levels.Where(x => levelsIds.Contains(x.Id)); //which one works ?


            var model = new TeacherDetailsViewModel
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Gender = teacher.Gender,
                Salary = teacher.Salary,
                HiringDate = teacher.HiringDate,
                DismissalDate = teacher.DismissalDate,
                QualifiedLevels = levels
            };

            return model;
        }

        public AllTeachersListViewModel GetAll()
        {
            var teachers = this.db.Teachers
                          .Select(teacher => new TeachersListDetailsViewModel
                          {
                              Id = teacher.Id,
                              FirstName = teacher.FirstName,
                              LastName = teacher.LastName,
                              Capacity = 0,
                              Efficiency = 0,
                              Groups = teacher.Groups.Select(x => x.Name).ToArray(),
                          })
                          .ToArray()
                          .OrderBy(x => x.Id)
                          .ToArray();

            var model = new AllTeachersListViewModel() { Teachers = teachers };
            return model;
        }

        public IEnumerable<TeacherDropDownViewModel> GetAllDropDown()
        {
            var list = this.db.Teachers.Select(x =>
                new TeacherDropDownViewModel
                {
                    Id=x.Id,
                    Name=x.FullName
                }).ToArray();

            return list;
        }

        public async Task<bool> TeacherExists(int teacherId)
        {
            return await this.db.Teachers.AnyAsync(x => x.Id == teacherId);
        }
    }
}
