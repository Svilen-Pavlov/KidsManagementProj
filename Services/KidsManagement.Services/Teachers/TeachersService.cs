using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
using KidsManagement.Services.External.CloudinaryService;
using KidsManagement.ViewModels.Groups;
using KidsManagement.ViewModels.Teachers;
using KidsManagement.ViewModels.Teachers.MyZoneModels;
using KidsManagement.ViewModels.Teachers.MyZoneModels.MySchedule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services.Teachers
{
    public class TeachersService : ApplicationUserService, ITeachersService
    {
        private readonly ICloudinaryService cloudinaryService;

        public TeachersService(KidsManagementDbContext db, ICloudinaryService cloudinaryService, UserManager<ApplicationUser> userManager) : base
            (db, userManager)
        {
            this.db = db;
            this.cloudinaryService = cloudinaryService;
            this.userManager = userManager;
        }


        public async Task<int> CreateTeacher(CreateEditTeacherInputModel model) 
        {
            var groupIds = model.Groups.Where(x => x.Selected).Select(x => x.Id).ToArray();
            var groupsToAssignToTeacher = this.db.Groups.Where(x => groupIds.Contains(x.Id)).ToArray(); 
            var levelsIds = model.Levels.Where(x => x.Selected).Select(x => x.Id).ToArray();
            var qualifiedLevels = this.db.Levels.Where(x => levelsIds.Contains(x.Id)).ToArray();

            //user side creation
            //User config = //https://stackoverflow.com/questions/34343599/how-to-seed-users-and-roles-with-code-first-migration-using-identity-asp-net-cor
            var user = new ApplicationUser
            {
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                UserName = model.Username,
                NormalizedUserName = model.Username.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var hashedPassword = passwordHasher.HashPassword(user, model.Password);
            user.PasswordHash = hashedPassword;

            var userStore = new UserStore<ApplicationUser>(this.db);
            var result = userStore.CreateAsync(user);

            var roles = new string[] { "TEACHER" };
            await base.AssignRoles(user.NormalizedUserName, roles);


            //business side creation
            var teacher = new Teacher
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                HiringDate = model.HiringDate,
                DismissalDate = model.DismissalDate,
                Salary = model.Salary,
                ProfilePicURI = model.ProfileImage == null ? Const.defProfPicURL : await this.cloudinaryService.UploadPicASync(model.ProfileImage, null),
                Groups = groupsToAssignToTeacher,
                QualifiedLevels = qualifiedLevels.Select(ql => new LevelTeacher { Level = ql }).ToArray(),
                Status= groupsToAssignToTeacher.Length==0 ? TeacherStatus.Initial:TeacherStatus.Active,
                ApplicationUserId = user.Id,
            };
            await this.db.Teachers.AddAsync(teacher);

            await this.db.SaveChangesAsync();

            return teacher.Id;
        }


        public TeacherDetailsViewModel FindById(int teacherId)
        {
            var teacher = this.db.Teachers
                .Include(t => t.ApplicationUser)
                .FirstOrDefault(x => x.Id == teacherId);
            var levelsIds = this.db.LevelTeachers.Where(x => x.TeacherId == teacherId).Select(x => x.LevelId).ToArray();
            var levels = this.db.Levels.Where(x => levelsIds.Contains(x.Id));
            var groups = this.db.Groups.Where(g => g.TeacherId == teacher.Id).Select(g => new GroupSelectionViewModel
            {
                Id = g.Id,
                Name = g.Name,
            }).ToList();


            string dissmissalDate = teacher.DismissalDate == null ? InfoStrings.GeneralNotSpecified : teacher.DismissalDate.Value.ToString(Const.dateOnlyFormat);

            string phoneNumber = InfoStrings.GeneralNotSpecified;
            string email = InfoStrings.GeneralNotSpecified;
            string username = InfoStrings.GeneralNotSpecified;

            if (string.IsNullOrEmpty(teacher.ApplicationUserId) == false)
            {
                phoneNumber = teacher.ApplicationUser.PhoneNumber;
                email = teacher.ApplicationUser.Email;
                username = teacher.ApplicationUser.UserName;
            }

            var model = new TeacherDetailsViewModel
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Gender = teacher.Gender,
                Salary = teacher.Salary,
                HiringDate = teacher.HiringDate.ToString(Const.dateOnlyFormat),
                DismissalDate = dissmissalDate,
                QualifiedLevels = levels,
                Status=teacher.Status,
                ProfilePicURI = teacher.ProfilePicURI,
                Groups = groups,
                Username = username,
                Email = email,
                PhoneNumber = phoneNumber
            };

            return model;
        }

        public AllTeachersListViewModel GetAll()
        {
            var teachers = this.db.Teachers
                          .Where(x=>x.Status!=TeacherStatus.Quit)
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

        public IEnumerable<TeacherSelectionViewModel> GetAllForSelection()
        {
            var list = this.db.Teachers
                .Where(x=>x.Status!=TeacherStatus.Quit)
                .Select(x =>
                new TeacherSelectionViewModel
                {
                    Id = x.Id,
                    Name = x.FullName
                }).ToArray();

            return list;
        }

        public async Task<bool> TeacherExists(int teacherId)
        {
            return await this.db.Teachers.Where(x=>x.Status!=TeacherStatus.Quit).AnyAsync(x => x.Id == teacherId);
        }

        public async Task<int> AddGroups(AddGroupsToTeacherViewModel model)
        {
            var teacher = await this.db.Teachers.FirstOrDefaultAsync(x => x.Id == model.TeacherId);

            var groupsIds = model.Groups.Where(x => x.Selected).Select(x => x.Id).ToArray();
            var groupsForTeacher = this.db.Groups.Where(x => groupsIds.Contains(x.Id)).ToArray();

            foreach (var group in groupsForTeacher)
            {
                group.TeacherId = teacher.Id;
            }
            teacher.Status = TeacherStatus.Active;

            return await this.db.SaveChangesAsync();
        }

        public async Task<int> GetBussinessIdByUserId(string userId)
        {
            var businessEntityId = (await this.db.Teachers.FirstOrDefaultAsync(u => u.ApplicationUserId == userId)).Id; // are we making use of async

            //var id = businessEntity.Id;

            return businessEntityId;
        }

        public TeacherMyZoneViewModel GetMyZoneInfo(int teacherId)
        {
            var teacher = this.db.Teachers
                .Include(t => t.Groups)
                .ThenInclude(g => g.Students)
                .FirstOrDefault(t => t.Id == teacherId);

            var model = new TeacherMyZoneViewModel();


            //Statistics
            model.Statistics.GroupsCount = teacher.Groups.Count().ToString();
            model.Statistics.StudentsCount = teacher.Groups.Sum(g => g.Students.Count).ToString();
            double weeklyMaxHours = 30;
            model.Statistics.WeeklyHoursCapacity = weeklyMaxHours.ToString(); //decide weather to leave it static TODO
            TimeSpan workHours = new TimeSpan(teacher.Groups.Sum(g => g.Duration.Ticks));
            model.Statistics.WorkingHours = workHours.ToString(Const.hourMinutesFormat);
            model.Statistics.Efficiency = string.Format("{0}%", Math.Round(workHours.TotalHours / weeklyMaxHours * 100, 2));



            //Notifications
            var notificationRangeStartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var notificationRangeEndDate = notificationRangeStartDate.AddDays(14);
            var notifications = new List<MyZoneNotification>();

            //groups
            //starting
            var groupsStarting = teacher.Groups
              .Where(g => g.StartDate >= notificationRangeStartDate && g.StartDate <= notificationRangeEndDate)
              .OrderBy(g => g.StartDate)
              .ToList();

            AddNotificationsGroup(notifications, groupsStarting, InfoStrings.MyZoneNotificationsGroupStart);
            //ending
            var groupsEnding = teacher.Groups
               .Where(g => g.EndDate >= notificationRangeStartDate && g.EndDate <= notificationRangeEndDate)
               .OrderBy(g => g.EndDate)
               .ToList();
            AddNotificationsGroup(notifications, groupsEnding, InfoStrings.MyZoneNotificationsGroupEnd);

            //birthdays
            var studentsWithBirthdays = teacher.Groups
                .SelectMany(g => g.Students)
                .Where(s => IsBirthDayInRange(s.BirthDate, notificationRangeStartDate, notificationRangeEndDate))
                .OrderBy(s => s.BirthDate)
                .ToArray();
            AddNotificationsBirthday(notifications, studentsWithBirthdays, InfoStrings.MyZoneNotificationsBirthday);

            model.Notifications = notifications;

            //Schedule


            return model;
        }

        public ScheduleViewModel GetMySchedule(int teacherId, DateTime inputDate, int marker)
        {
            var fromDate = DateTimeExtensions.StartOfWeek(inputDate.Date, DayOfWeek.Sunday);

            if (marker != 0)
                fromDate=fromDate.AddDays(marker * 7);
            

            var toDate = fromDate.AddDays(6);

            var groups = this.db.Groups
                .Include(g => g.Students) //needed only for studentscount in timeSlot
                .Where(g => g.TeacherId == teacherId)
                .Where(g => (g.StartDate>toDate || g.EndDate<fromDate) ==false)
                .OrderBy(g => g.DayOfWeek)
                .ToArray();

            var scheduleWeekDays = new List<ScheduleWeekDayViewModel>();

            for (int i = 0; i < 7; i++) //weekdays
            {
                var scheduleWeekday = new ScheduleWeekDayViewModel() { DayOfWeek = fromDate.AddDays(i).DayOfWeek };

                //if (groups.Any(g => g.DayOfWeek == scheduleWeekday.DayOfWeek) == false) continue;

                var dayOfWeekTimeSlots = new List<DayOfWeekTimeSlot>();
                var groupsForTheDay = groups
                    .Where(g => g.DayOfWeek == scheduleWeekday.DayOfWeek)
                    .OrderBy(g => g.StartTime)
                    .ToArray();


                for (int j = 0; j < groupsForTheDay.Count(); j++) //timeslots
                {
                    var group = groups[i];
                    var timeSlot = new DayOfWeekTimeSlot()
                    {
                        GroupName = group.Name,
                        StartTime = group.StartTime.ToString(Const.hourMinutesFormat),
                        EndTime = group.EndTime.ToString(Const.hourMinutesFormat),
                        Duration = group.Duration.ToString(Const.hourMinutesFormat),
                        StartDate = group.StartDate.ToString(Const.dateOnlyFormat),
                        EndDate = group.EndDate.ToString(Const.dateOnlyFormat),
                        StudentsCount = group.Students.Count(),
                        //KnownColorName=group.KnownColorName  TODO:Color in groups entity 

                    };
                    dayOfWeekTimeSlots.Add(timeSlot);
                }

                scheduleWeekday.TimeSlots = dayOfWeekTimeSlots;

                scheduleWeekDays.Add(scheduleWeekday);
            }





            var model = new ScheduleViewModel()
            {
                FromDateDisplay = fromDate.ToString(Const.dateOnlyFormat),
                ToDateDisplay = toDate.ToString(Const.dateOnlyFormat),
                FromDate = fromDate,
                ToDate = toDate,
                ScheduleWeekDays = scheduleWeekDays
            };

            return model;
        }


        public static void AddNotificationsGroup(List<MyZoneNotification> notifications, IEnumerable<Group> groups, string content)
        {
            foreach (var group in groups)
            {
                var notification = new MyZoneNotification()
                {
                    Type = MyZoneNotificationType.Group,
                    Content = string.Format(content, group.Name, group.StartDate.Day, group.StartDate.Month)
                };
                notifications.Add(notification);
            }
        }

        public static void AddNotificationsBirthday(List<MyZoneNotification> notifications, IEnumerable<Student> students, string content)
        {
            foreach (var student in students)
            {
                var newAge = DateTime.Now.Year - student.BirthDate.Year;
                var notification = new MyZoneNotification()
                {
                    Type = MyZoneNotificationType.Birthday,
                    Content = string.Format(content, student.FullName, student.Group.Name, newAge, student.BirthDate.Day, student.BirthDate.Month)
                };

                notifications.Add(notification);
            }
        }


        public async Task<CreateEditTeacherInputModel> GetInfoForEdit(int teacherId)
        {
            var teacher = await this.db.Teachers
               .FirstOrDefaultAsync(x => x.Id == teacherId);

            var model = new CreateEditTeacherInputModel
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Gender = teacher.Gender,
                Salary=teacher.Salary,
                HiringDate=teacher.HiringDate,
                DismissalDate=teacher.DismissalDate,
                ProfilePicURI = teacher.ProfilePicURI,
            };

            return model;
        }

        public async Task EditInfo(CreateEditTeacherInputModel model)
        {
            var teacher = await this.db.Teachers
               .FirstOrDefaultAsync(x => x.Id == model.Id);

            teacher.FirstName = model.FirstName;
            teacher.LastName = model.LastName;
            teacher.Gender = model.Gender;
            teacher.Salary = model.Salary;
            teacher.HiringDate = model.HiringDate;
            teacher.DismissalDate = model.DismissalDate;
            teacher.ProfilePicURI = model.ProfileImage == null ? Const.defProfPicURL : await this.cloudinaryService.UploadPicASync(model.ProfileImage, null);
        }

        public async Task<int> Delete(int teacherId)
        {
            var teacher = await this.db.Teachers
                .Include(t => t.Groups)
                .FirstOrDefaultAsync(s => s.Id == teacherId);

            if (teacher.Status!=TeacherStatus.Active)
            teacher.Status = TeacherStatus.Quit;


            return this.db.SaveChangesAsync().Result;
        }
        public static bool IsBirthDayInRange(DateTime birthday, DateTime start, DateTime end)
        {
            DateTime temp = birthday.AddYears(start.Year - birthday.Year).Date;

            if (temp < start)
                temp = temp.AddYears(1);

            return birthday.Date <= end && temp >= start && temp <= end;
        }

        public async Task<int> UnassignGroup(int teacherId,int groupId)
        {
            var teacher = await this.db.Teachers.Include(t=>t.Groups).FirstOrDefaultAsync(g => g.Id == teacherId);
            var group = await this.db.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            teacher.Groups.Remove(group);
            if (teacher.Groups.Count == 0)
                teacher.Status = TeacherStatus.Inactive;

            return await this.db.SaveChangesAsync();
        }
        public static bool IsDateInRange(DateTime groupStart, DateTime groupEnd, DateTime fromDate, DateTime toDate)
        {
            return (groupStart < toDate || groupEnd > fromDate) && (groupStart > fromDate || groupEnd > toDate);
        }

    }
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff);
        }
    }
}
