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


        public async Task<int> CreateGroup(CreateEditGroupInputModel model)
        {
            DateTime startDate = model.StartDate ?? DateTime.Now;
            DateTime endDate = model.EndDate ?? DateTime.Now;
            TimeSpan duration = TimeSpan.FromMinutes(model.Duration);

            var group = new Group
            {
                Name = model.Name,
                AgeGroup = model.AgeGroup,
                CurrentLessonNumber = 1, //0 and add logic for increasing lesson numbers
                DayOfWeek = model.DayOfWeek,
                Duration = duration,
                StartDate = startDate,
                EndDate = endDate,
                StartTime = model.StartTime,
                EndTime = model.StartTime.Add(duration),
                LevelId = model.LevelId,
                Status = GroupStatus.Empty,
                ActiveStatus = GroupActiveStatus.Initial,
                MaxStudents = (int)model.AgeGroup
            };

            if (model.TeacherId != 0)
                group.TeacherId = model.TeacherId;


            await this.db.Groups.AddAsync(group);
            await this.db.SaveChangesAsync();

            return group.Id;
        }

        public GroupDetailsViewModel FindById(int groupId)
        {
            var group = this.db.Groups
                //.Include(g=>g.Teacher) optimize DB operation include case if group is teacherless 
                .FirstOrDefault(g => g.Id == groupId && g.ActiveStatus != GroupActiveStatus.Quit);
            var level = this.db.Levels.FirstOrDefault(x => x.Id == group.LevelId); //dali 6e go nameri
            var students = this.db.Students.Where(x => x.GroupId == group.Id).ToArray(); //should it be Included? optimize
            var model = new GroupDetailsViewModel
            {
                Id = group.Id,
                Name = group.Name,
                CurrentLessonNumber = group.CurrentLessonNumber,
                AgeGroup = group.AgeGroup == 0 ? InfoStrings.GeneralNotSpecified : group.AgeGroup.ToString(),
                DayOfWeek = group.DayOfWeek,
                Duration = group.Duration.ToString(Constants.hourMinutesFormat),
                StartDate = group.StartDate.ToString(Constants.dateOnlyFormat),
                EndDate = group.EndDate.ToString(Constants.dateOnlyFormat),
                StartTime = group.StartTime.ToString(Constants.hourMinutesFormat),
                EndTime = group.EndTime.ToString(Constants.hourMinutesFormat),
                LevelId = (int)group.LevelId,
                LevelName = level.Name,
                ActiveStatus = group.ActiveStatus,
                MaxStudents = (int)group.MaxStudents,
                Students = students.Select(s => new AllSingleStudentsViewModel()
                {
                    FullName = s.FullName,
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
        public async Task<bool> AddStudentToGroup(int groupId, int studentId)
        {
            var student = await this.db.Students.FirstOrDefaultAsync(x => x.Id == studentId);
            var group = await this.db.Groups
                .Include(x=>x.Students)
                .FirstOrDefaultAsync(x => x.Id == groupId);

            group.Students.Add(student);
            student.Status = StudentStatus.Active;
            bool isGroupFull = true;

            if (group.MaxStudents == group.Students.Count)
                group.Status = GroupStatus.Full;
            else if (group.Students.Count>group.MaxStudents)
            {
                group.Status = GroupStatus.OverLimit;
            }
            else
            {
                isGroupFull = false;
            }
            await this.db.SaveChangesAsync();
            return isGroupFull;
        }
        public async Task<bool> IsGroupFull(int groupId)
        {
            var notFullStatuses = new List<GroupStatus> { GroupStatus.NotFull, GroupStatus.Empty };
            var group = await this.db.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
            return notFullStatuses.Contains(group.Status);
        } //do i need this?

        public async Task RemoveStudentFromGroup(int studentId)
        {
            var student = await this.db.Students
                .Include(s => s.Group)
                .FirstOrDefaultAsync(x => x.Id == studentId);

            student.Group.Students.Remove(student);

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
            return await this.db.Groups.AnyAsync(g => g.Id == groupId && g.ActiveStatus != GroupActiveStatus.Quit);
        }

        public AllGroupsDetailsViewModel GetAll()
        {
            return this.GetAll(0);
        }

        public AllGroupsDetailsViewModel GetAll(int teacherId)
        {
            var groups = this.db.Groups
                .Include(g => g.Level)
                .Include(g => g.Teacher)
                .Include(g => g.Students)
                .Where(g => (teacherId != 0) ? g.TeacherId == teacherId : true && g.ActiveStatus != GroupActiveStatus.Quit) //skip where if teacherId==0 ; https://stackoverflow.com/questions/3682835/if-condition-in-linq-where-clause
                .Select(g => new SingleGroupDetailsViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    DayOfWeek = g.DayOfWeek,
                    StartTime = g.StartTime.ToString(Constants.hourMinutesFormat),
                    LevelName = g.Level.Name,
                    TeacherName = g.Teacher == null ? InfoStrings.GroupHasNoTeacherYet : g.Teacher.FullName, //make it the same as in other methods - FindById
                    StudentsCount = g.Students.Count(),
                    ActiveStatus = g.ActiveStatus,
                    MaxStudentsCount = g.MaxStudents
                })
                .ToArray()
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.StartTime)
                .ThenByDescending(x => x.StudentsCount)
                .ToList();


            var model = new AllGroupsDetailsViewModel() { Groups = groups };

            return model;
        }

        public AllGroupsOfTeacherViewModel GetGroupsByTeacher(int teacherId)
        {

            var groups = this.db.Groups
                .Where(g => g.TeacherId == teacherId)
                .Include(g => g.Students)
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
                    ActiveStatus = g.ActiveStatus,
                    Status = g.Status,
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
            var groups = this.db.Groups.Where(g => g.TeacherId == teacherId && g.ActiveStatus != GroupActiveStatus.Quit)
                .Select(g => new GroupSelectionViewModel
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
                groups = this.db.Groups.Where(g => g.TeacherId == null && g.ActiveStatus != GroupActiveStatus.Quit)
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
            var student = await this.studentsService.FindById(studentId); // optimize this so that we don't need another call too the DB (only needed properties are student Id and grade)

            var grade = (int)student.Grade;
            var ageGroup = new AgeGroup();
            if (grade < 5) ageGroup = AgeGroup.Preschool;
            else if (grade < 9) ageGroup = AgeGroup.Primary;
            else if (grade < 12) ageGroup = AgeGroup.Secondary;
            else ageGroup = AgeGroup.High;


            var groups = this.db.Groups
                .Include(g => g.Teacher)
                .Include(g => g.Level)
                .Include(g => g.Students)
                .Where(g => g.AgeGroup == ageGroup && (int)g.Status < 3 || g.Students.Any(s => s.Id == student.Id) && g.ActiveStatus!=GroupActiveStatus.Quit)
                .ToArray()
                .Select(g => new SingleGroupDetailsViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    TeacherName = g.TeacherId == null ? InfoStrings.GroupHasNoTeacherYet : g.Teacher.FullName,
                    AgeGroup = g.AgeGroup,
                    DayOfWeek = g.DayOfWeek,
                    LevelName = g.Level.Name,
                    StartTime = g.StartTime.ToString(Constants.hourMinutesFormat),
                    StudentsCount = g.Students.Count(),
                    MaxStudentsCount = g.MaxStudents,
                    ActiveGroup = student.GroupId == g.Id ? true : false
                })
                .OrderByDescending(g => g.ActiveGroup)
                .ThenBy(g => g.DayOfWeek)
                .ThenBy(g => g.StartTime)
                .ThenBy(g => g.StudentsCount)
                .ToArray();



            return await Task.FromResult(groups); //https://stackoverflow.com/questions/25182011/why-async-await-allows-for-implicit-conversion-from-a-list-to-ienumerable
        }

        public AllGroupsOfTeacherViewModel GetActiveGroupsByTeacher(int teacherId)
        {
            var statuses = new List<GroupActiveStatus> { GroupActiveStatus.Started, GroupActiveStatus.Paused };
            var model = this.GetGroupsByTeacher(teacherId);
            var filtered = model.Groups.ToList();
            filtered.RemoveAll(x => statuses.Contains(x.ActiveStatus) == false);
            model.Groups = filtered;

            return model;
        }

        public async Task<CreateEditGroupInputModel> GetInfoForEdit(int groupId)
        {
            var group = await this.db.Groups
               .FirstOrDefaultAsync(x => x.Id == groupId);

            var model = new CreateEditGroupInputModel
            {
                Name = group.Name,
                AgeGroup = group.AgeGroup,
                DayOfWeek = group.DayOfWeek,
                Duration = (int)group.Duration.TotalMinutes,
                StartDate = group.StartDate,
                EndDate = group.EndDate,
                StartTime=group.StartTime,
                
            };

            return model;
        }

        public async Task EditInfo(CreateEditGroupInputModel model)
        {
            DateTime StartDate = model.StartDate ?? DateTime.Now;
            DateTime EndDate = model.EndDate ?? DateTime.Now;
            TimeSpan duration = TimeSpan.FromMinutes(model.Duration);


            var group = await this.db.Groups.FirstOrDefaultAsync(x => x.Id == model.Id);

            group.Name = model.Name;
            group.AgeGroup = model.AgeGroup;
            group.StartDate = StartDate;
            group.EndDate = EndDate;
            group.StartTime = model.StartTime;
            group.Duration = duration;
            group.EndTime = model.StartTime.Add(duration);
            group.DayOfWeek = model.DayOfWeek;

            await this.db.SaveChangesAsync();
        }

        public async Task<int> Delete(int groupId)
        {
            var group = await this.db.Groups
                .Include(g=>g.Students)
               .FirstOrDefaultAsync(s => s.Id == groupId);
            var students = group.Students.ToArray();

            for (int i = 0; i < students.Length; i++)
            {
                group.Students.Remove(students[i]);
            }

            group.ActiveStatus = GroupActiveStatus.Quit;

            var result = await this.db.SaveChangesAsync();
            return result;
        }
    }
}
