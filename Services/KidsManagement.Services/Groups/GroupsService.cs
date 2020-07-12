using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
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
            //var teacher = this.db.Teachers.FirstOrDefault(x => x.Id == input.TeacherId);
            //var level = this.db.Levels.FirstOrDefault(x => x.Id == input.LevelId);
            //string name = $"{teacher.FirstName} {input.DayOfWeek.ToString().ToUpper().Take(3)} {input.StartTime}";

            var group = new Group
            {
                TeacherId=input.TeacherId,
                StartTime = input.StartTime,
                Name = input.Name,
                AgeGroup = input.AgeGroup,
                CurrentLessonNumber = 1,
                DayOfWeek = input.DayOfWeek,
                Duration = input.EndTime.Subtract(input.StartTime),
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                EndTime = input.EndTime,
                LevelId = input.LevelId,

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
                LevelId = (int)group.LevelId,
                LevelName = level.Name,
                TeacherId = (int)group.TeacherId,
                TeacherName = teacher.FullName,
                MaxStudents=(int)group.MaxStudents,
                Students = students.Select(s => new AllSingleStudentsViewModel()
                {
                    FulLName = s.FullName,
                    Age = s.Age,
                    Gender = s.Gender,
                    GroupName = group.Name,
                    Id = s.Id
                }
                ).ToArray()
            };

            return model;
        }
        public async Task AddStudent(int studentId, int groupId)
        {
            var student = await this.db.Students.FirstOrDefaultAsync(x => x.Id == studentId);
            student.GroupId = groupId;
            await this.db.SaveChangesAsync();
        }

        public async Task RemoveStudent(int studentId, int groupId)
        {
            var student = await this.db.Students.FirstOrDefaultAsync(x => x.Id == studentId);
            student.GroupId = 0;
            await this.db.SaveChangesAsync();
        }

        public void ChangeTeacher(int newTeacherId, int groupId) //teacher service or here?
        {
            var group = this.db.Groups.FirstOrDefault(x => x.Id == groupId);
            group.TeacherId = newTeacherId;
            this.db.SaveChanges();
        }

        public async Task<bool> GroupExists(int groupId)
        {
            return await this.db.Groups.AnyAsync(x => x.Id == groupId);
        }

        public AllGroupsDetailsViewModel GetAll()
        {
            var groups = this.db.Groups
                .Include(x => x.Level)
                .Include(x => x.Teacher)
                .Select(x => new SingleGroupDetailsViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    DayOfWeek = x.DayOfWeek,
                    StartTime = x.StartTime,
                    LevelName = x.Level.Name,
                    TeacherName = x.Teacher.FullName
                })
                .ToArray()
                .OrderBy(x => x.TeacherName);


            var groupsList = new List<SingleGroupDetailsViewModel>(groups);

            var model = new AllGroupsDetailsViewModel() { Groups = groupsList };

            return model;
        }

        public async Task<bool> GroupIsFull(int groupId)
        {
            var studentsCount = this.db.Students.Where(x => x.GroupId == groupId).Count();
            var group = await this.db.Groups.FirstOrDefaultAsync(x => x.Id == groupId);

            return studentsCount == group.MaxStudents;
        }

        public AllGroupsOfTeacherViewModel GetAllByTeacher(int teacherId)
        {

            var groups = this.db.Groups
                .Where(g => g.TeacherId == teacherId)
                //.Include(x=>x)
                .Select(g => new SingleGroupOfTeacherDetailsViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    CurrentLessonNumber = g.CurrentLessonNumber,
                    AgeGroup=g.AgeGroup,
                    StartDate=g.StartDate,
                    EndDate=g.EndDate,
                    DayOfWeek = g.DayOfWeek,
                    Duration = g.Duration,
                    StartTime = g.StartTime,
                    EndTime = g.EndTime,
                    TeacherId=teacherId,
                    LevelId=g.Level.Id,
                    Capacity=g.MaxStudents,
                    Efficiency=Math.Round((double)g.Students.Count()/g.MaxStudents*100,2)
                    //TODO:
                    //Teacher
                    //Level
                    //Students 

                })
                .ToArray()
                .OrderBy(t => t.Name);


            var teacherName = this.db.Teachers.FirstOrDefault(x => x.Id == teacherId).FullName;
            var groupsList = new List<SingleGroupOfTeacherDetailsViewModel>(groups);

            var model = new AllGroupsOfTeacherViewModel() { Groups = groupsList,TeacherName= teacherName };

            return model;
        }

       

    }
}
