using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.ViewModels.Groups;
using KidsManagement.ViewModels.Students;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services.Groups
{

    public class GroupsService : IGroupsService
    {
        private readonly KidsManagementDbContext db;

        public GroupsService(KidsManagementDbContext db)
        {
            this.db = db;
        }


        public async Task<int> CreateGroup(GroupCreateInputModel input)
        {
            var teacher = this.db.Teachers.FirstOrDefault(x => x.Id == input.TeacherId);
            var level = this.db.Levels.FirstOrDefault(x => x.Id == input.LevelId);
            string name = $"{teacher.FirstName} {input.DayOfWeek.ToString().ToUpper().Take(3)} {input.StartTime}";

            var group = new Group
            {
                Teacher = teacher,
                StartTime = input.StartTime,
                Name = name,
                AgeGroup = input.AgeGroup,
                CurrentLessonNumber = 1,
                DayOfWeek = input.DayOfWeek,
                Duration = input.Duration,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                EndTime = input.EndTime,
                Level = level,
                CreatedOn = DateTime.UtcNow,
            };

            await this.db.Groups.AddAsync(group);
            await this.db.SaveChangesAsync();

            return group.Id;
        }

        public GroupDetailsViewModel FindById(int groupId)
        {
            var group = this.db.Groups.FirstOrDefault(x => x.Id == groupId);
            var level = this.db.Levels.FirstOrDefault(x => x.Id == group.LevelId); //dali 6e go nameri
            var teacher = this.db.Teachers.FirstOrDefault(x => x.Id == group.TeacherId); //dali 6e go nameri
            var students = this.db.Students.Where(x => x.GroupId == group.Id).ToArray();
            var model = new GroupDetailsViewModel
            {
                Id = group.Id,
                Name = group.Name,
                CurrentLessonNumber = group.CurrentLessonNumber,
                AgeGroup = group.AgeGroup,
                DayOfWeek = group.DayOfWeek,
                Duration = group.Duration.ToString(@"hh\:mm"),
                StartDate = group.StartDate.ToString("d"),
                EndDate = group.EndDate.ToString("d"),
                StartTime = group.StartTime.ToString(@"hh\:mm"),
                EndTime = group.EndTime.ToString(@"hh\:mm"),
                LevelId = group.LevelId,
                LevelName = level.Name,
                TeacherId = group.TeacherId,
                TeacherName = teacher.FullName,
                Students = students.Select(s=>new AllSingleStudentsViewModel()
                { 
                    FulLName=s.FullName,
                    Age=s.Age,
                    Gender=s.Gender,
                    GroupName=group.Name,
                    Id=s.Id
                }
                ).ToArray()
            };

            return model;
        }
        public void AddStudent(int studentId, int groupId)
        {
            var student = this.db.Students.FirstOrDefault(x => x.Id == studentId && x.IsDeleted == false);
            student.GroupId = groupId;
            student.LastModified = DateTime.UtcNow;
            this.db.SaveChanges();
        }

        public void RemoveStudent(int studentId, int groupId)
        {
            var student = this.db.Students.FirstOrDefault(x => x.Id == studentId && x.IsDeleted == false);
            student.GroupId = 0;
            student.LastModified = DateTime.UtcNow;
            this.db.SaveChanges();
        }

        public void ChangeTeacher(int newTeacherId, int groupId) //teacher service or here?
        {
            var group = this.db.Groups.FirstOrDefault(x => x.Id == groupId);
            group.TeacherId = newTeacherId;
            this.db.SaveChanges();
        }

        public bool GroupExists(int groupId)
        {
            return this.db.Groups.Any(x => x.Id == groupId);
        }

        public AllGroupsDetailsViewModel GetAll()
        {
            var groups = this.db.Groups
                .Include(x => x.Level)
                .Include(x=>x.Teacher)
                .Select(x => new AllSinglegroupDetailsViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    DayOfWeek = x.DayOfWeek,
                    StartTime = x.StartTime,
                    LevelName = x.Level.Name,
                    TeacherName=x.Teacher.FullName
                })
                .ToArray()
                .OrderBy(x=>x.TeacherName);


            var groupsList = new List<AllSinglegroupDetailsViewModel>(groups);

            var model = new AllGroupsDetailsViewModel(){ Groups = groupsList };

            return model;
        }
    }
}
