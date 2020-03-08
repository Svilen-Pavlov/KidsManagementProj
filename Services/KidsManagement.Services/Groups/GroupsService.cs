using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.ViewModels.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KidsManagement.Services.Groups
{

    public class GroupsService : IGroupsService
    {
        private readonly KidsManagementDbContext db;

        public GroupsService(KidsManagementDbContext db)
        {
            this.db = db;
        }

        public void AddStudent(int studentId, int groupId)
        {
            var student = this.db.Students.FirstOrDefault(x => x.Id == studentId);
            student.GroupId = groupId;
            this.db.SaveChanges();
        }

        public void Create(GroupCreateInputModel input)
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

            this.db.Groups.Add(group);
            this.db.SaveChanges();

        }

        public GroupDetailsViewModel FindById(int id)
        {
            var group = this.db.Groups.FirstOrDefault(x => x.Id == id);
            var level = this.db.Levels.FirstOrDefault(x => x.Id == group.LevelId);
            var teacher = this.db.Teachers.FirstOrDefault(x => x.Id == group.TeacherId);
            var students = this.db.Students.Where(x => x.GroupId == group.Id).ToArray();
            var model = new GroupDetailsViewModel
            {
                Id = group.Id,
                Name = group.Name,
                CurrentLessonNumber = group.CurrentLessonNumber,
                AgeGroup = group.AgeGroup,
                DayOfWeek = group.DayOfWeek,
                Duration = group.Duration,
                EndDate = group.EndDate,
                EndTime = group.EndTime,
                LevelId = group.LevelId,
                LevelName = level.Name,
                StartDate = group.StartDate,
                StartTime = group.StartTime,
                TeacherId = group.TeacherId,
                TeacherName = teacher.FullName,
                Students = students
            };

            return model;
        }

        public void RemoveStudent(int studentId, int groupId)
        {
            var student = this.db.Students.FirstOrDefault(x => x.Id == studentId);

            student.GroupId = 0;
            this.db.SaveChanges();
        }
    }
}
