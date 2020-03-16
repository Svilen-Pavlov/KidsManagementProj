using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.ViewModels.Groups;
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
                Duration = group.Duration,
                EndDate = group.EndDate,
                EndTime = group.EndTime,
                LevelId = group.LevelId,
                LevelName = level.Name,
                StartDate = group.StartDate,
                StartTime = group.StartTime,
                TeacherId = group.TeacherId,
                TeacherName = teacher.FullName,
                //Students = new students
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
                .Include(x => x.Students)
                .Include(x=>x.Teacher)
                .Select(x => new GroupDetailsViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    AgeGroup = x.AgeGroup,
                    CurrentLessonNumber = x.CurrentLessonNumber,
                    DayOfWeek = x.DayOfWeek,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Duration = x.Duration,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    
                    LevelId = x.LevelId,
                    LevelName = x.Level.Name,
                    //Students = x.Students,
                    TeacherId=x.TeacherId,
                    TeacherName=x.Teacher.FullName
                }).ToArray();


            var groupsList = new List<GroupDetailsViewModel>(groups);

            var model = new AllGroupsDetailsViewModel(){ Groups = groupsList };

            return model;
        }
    }
}
