using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
using KidsManagement.Services.Students;
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
        private readonly IStudentsService studentsService;

        public GroupsService(KidsManagementDbContext db, IStudentsService studentsService)
        {
            this.db = db;
            this.studentsService = studentsService;
        }


        public async Task<int> CreateGroup(CreateGroupInputModel model)
        {
            //a new non teacher group

            var group = new Group
            {
                StartTime = model.StartTime,
                Name = model.Name,
                AgeGroup = model.AgeGroup,
                CurrentLessonNumber = 1,
                DayOfWeek = model.DayOfWeek,
                Duration = model.EndTime.Subtract(model.StartTime),
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                EndTime = model.EndTime,
                LevelId = model.LevelId,
                Status = GroupStatus.Empty,
                MaxStudents = (int)model.AgeGroup
            };

            if(model.TeacherId!=0)
                group.TeacherId = model.TeacherId;
            

            await this.db.Groups.AddAsync(group);
            await this.db.SaveChangesAsync();

            return group.Id;
        }

        public GroupDetailsViewModel FindById(int groupId)
        {
            var group = this.db.Groups
                //.Include(g=>g.Teacher) optimize DB operation
                .FirstOrDefault(x => x.Id == groupId);
            var level = this.db.Levels.FirstOrDefault(x => x.Id == group.LevelId); //dali 6e go nameri
            var students = this.db.Students.Where(x => x.GroupId == group.Id).ToArray();
            var model = new GroupDetailsViewModel
            {
                Id = group.Id,
                Name = group.Name,
                CurrentLessonNumber = group.CurrentLessonNumber,
                AgeGroup = group.AgeGroup==0? InfoStrings.GeneralNotSpecified:group.AgeGroup.ToString(),
                DayOfWeek = group.DayOfWeek,
                Duration = group.Duration.ToString(Const.hourMinutesFormat),
                StartDate = group.StartDate.ToString(Const.dateOnlyFormat),
                EndDate = group.EndDate.ToString(Const.dateOnlyFormat),
                StartTime = group.StartTime.ToString(Const.hourMinutesFormat),
                EndTime = group.EndTime.ToString(Const.hourMinutesFormat),
                LevelId = (int)group.LevelId,
                LevelName = level.Name,
                MaxStudents = (int)group.MaxStudents,
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

            var teacher = this.db.Teachers.FirstOrDefault(x => x.Id == group.TeacherId); //optimize database operation
            if ((teacher == null) == false)
            {
                model.TeacherId = teacher.Id;
                model.TeacherName = teacher.FullName;
            }
            else
            {
                model.TeacherName = InfoStrings.GroupHasNoTeacherYet;
            }



            return model;
        }
        public async Task AddStudent(int studentId, int groupId)
        {
            var student = await this.db.Students.FirstOrDefaultAsync(x => x.Id == studentId);
            student.GroupId = groupId;
            student.Status = StudentStatus.Active;

            await this.db.SaveChangesAsync();
        }

        public async Task RemoveStudent(int studentId, int groupId)
        {
            var student = await this.db.Students.FirstOrDefaultAsync(x => x.Id == studentId);
            student.GroupId = 0;
            student.Status = StudentStatus.Inactive;
            await this.db.SaveChangesAsync();
        }

        public void ChangeTeacher(int newTeacherId, int groupId)
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
                .Include(g => g.Level)
                .Include(g => g.Teacher)
                .Include(g=>g.Students)
                .Select(g => new SingleGroupDetailsViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    DayOfWeek = g.DayOfWeek,
                    StartTime = g.StartTime.ToString(Const.hourMinutesFormat),
                    LevelName = g.Level.Name,
                    TeacherName = g.Teacher==null? InfoStrings.GroupHasNoTeacherYet : g.Teacher.FullName, //make it the same as in other methods - FindById
                    StudentsCount=g.Students.Count()
                })
                .ToArray()
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.StartTime)
                .ThenByDescending(x => x.StudentsCount);


            var groupsList = new List<SingleGroupDetailsViewModel>(groups);

            var model = new AllGroupsDetailsViewModel() { Groups = groupsList };

            return model;
        }


        public AllGroupsOfTeacherViewModel GetAllByTeacher(int teacherId)
        {

            var groups = this.db.Groups
                .Where(g => g.TeacherId == teacherId)
                .Include(g=>g.Students)
                .Select(g => new SingleGroupOfTeacherDetailsViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    CurrentLessonNumber = g.CurrentLessonNumber,
                    AgeGroup = g.AgeGroup,
                    StartDate = g.StartDate,
                    EndDate = g.EndDate,
                    DayOfWeek = g.DayOfWeek,
                    Duration = g.Duration,
                    StartTime = g.StartTime,
                    EndTime = g.EndTime,
                    TeacherId = teacherId,
                    LevelId = g.Level.Id,
                    Capacity = g.MaxStudents,
                    Efficiency = Math.Round((double)g.Students.Count() / g.MaxStudents * 100, 2)
                    //TODO:
                    //Teacher
                    //Level
                    //Students 

                })
                .ToArray()
                .OrderBy(t => t.Name);


            var teacherName = this.db.Teachers.FirstOrDefault(x => x.Id == teacherId).FullName;
            var groupsList = new List<SingleGroupOfTeacherDetailsViewModel>(groups);

            var model = new AllGroupsOfTeacherViewModel() { Groups = groupsList, TeacherName = teacherName };

            return model;
        }

        /// <summary>
        /// Gets all groups that are assigned to a specific teacher by teacherId
        /// </summary>
        public IEnumerable<GroupSelectionViewModel> GetAllForSelection(int teacherId) 
        {
                var groups = this.db.Groups.Where(g => g.TeacherId == teacherId).Select(g => new GroupSelectionViewModel
                {
                    Id = g.Id,
                    Name = g.Name
                }).ToList();

            return groups;
        }

        /// <summary>
        /// Gets all groups including those with an already assigned teacher (true) or only those without one (false)
        /// </summary>
        public IEnumerable<GroupSelectionViewModel> GetAllForSelection(bool includingGroupsWithAssignedTeacher)
        {

            List<GroupSelectionViewModel> groups = new List<GroupSelectionViewModel>();
            if (includingGroupsWithAssignedTeacher)
            {
                groups = this.db.Groups.Select(g => new GroupSelectionViewModel
                {
                    Id = g.Id,
                    Name = g.Name
                }).ToList();
            }
            else
            {
                groups = this.db.Groups.Where(g => g.TeacherId == null)
                    .Select(g => new GroupSelectionViewModel
                    {
                        Id = g.Id,
                        Name = g.Name
                    })
                    .ToList();
            }

            return groups;
        }

        public async Task<IEnumerable<SingleGroupDetailsViewModel>> GetVacantGroupsWithProperAge(int studentId)
        {
            var student=await this.studentsService.FindById(studentId); // optimize this so that we don't need another call too the DB (only needed properties are student Id and grade)

            var grade = (int)student.Grade;
            var ageGroup = new AgeGroup();
            if (grade < 5) ageGroup = AgeGroup.Preschool;
            else if (grade < 9) ageGroup = AgeGroup.Primary;
            else if (grade < 12) ageGroup = AgeGroup.Secondary;
            else ageGroup = AgeGroup.High;


            var groups = this.db.Groups
                .Include(g=>g.Teacher)
                .Include(g=>g.Level)
                .Include(g=>g.Students)
                .Where(g => g.AgeGroup == ageGroup && (int)g.Status<3)
                .ToArray()
                .Select(g=> new SingleGroupDetailsViewModel
                {
                    Id=g.Id,
                    Name=g.Name,
                    TeacherName=g.TeacherId==null? InfoStrings.GroupHasNoTeacherYet : g.Teacher.FullName,
                    AgeGroup=g.AgeGroup,
                    DayOfWeek=g.DayOfWeek,
                    LevelName=g.Level.Name,
                    StartTime=g.StartTime.ToString(Const.hourMinutesFormat),
                    StudentsCount=g.Students.Count(),
                    MaxStudentsCount=g.MaxStudents,
                })
                .OrderBy(g=>g.DayOfWeek)
                .ThenBy(g=>g.StartTime)
                .ThenBy(g=>g.StudentsCount)
                .ToArray();

            

            //https://stackoverflow.com/questions/25182011/why-async-await-allows-for-implicit-conversion-from-a-list-to-ienumerable
            return await Task.FromResult(groups);
        }
    }
}
