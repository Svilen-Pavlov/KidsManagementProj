using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using KidsManagement.Data;
using KidsManagement.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using KidsManagement.Services.Groups;
using KidsManagement.Services.Students;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace KidsManagementConsole
{
    public class Program
    {
        public static void Main()
        {
            var db = new KidsManagementDbContext();
            //var gs = new GroupsService(db);
            //var result = gs.GetAll();
            //Console.WriteLine(gs.GetAll().ToString());

            //db.Database.Migrate();
            //SeedParentsTeachersAdmins(db);
            //SeedLevels(db);
            //SeedGroups(db);
            //SeedStudents(db);
            //AddStudentsToGroup(db);
            //SeedRoles(db);
            //UpdateGroupStatuses(db);
            //UpdateGroupAgeGroups(db);
            //UpdateStudentStatuses(db);
            //UpdateStudentGrades(db);

            AssignStudentsToParents(db);
            //UpdateParentsStatuses(db); does nothing if the item below is not met
            //write a method to asign parents to children

            //testGettingPicString(); TOREMOVE
            //UpdateTeachersStatuses(db);
            //UpdateGroupActiveStatuses(db);
        }

        private static void AssignStudentsToParents(KidsManagementDbContext db)
        {
            var amountOfParents = 5;
            var amountOfStudents = 10;

            var parentIds = db.Parents
                .Take(amountOfParents)
                .Select(p => p.Id)
                .ToArray();

            var students = db.Students
                .Take(amountOfStudents)
                .ToArray();

            var counter = 0;


            for (int i = 0; i < parentIds.Length; i++)
            {
                var parentId = parentIds[i];

                var student1 = students[counter];
                var student2 = students[counter+1];

                var link = new StudentParent { ParentId = parentId };
                var link2 = new StudentParent { ParentId = parentId };
                
                student1.Parents.Add(link);
                student2.Parents.Add(link2);
                counter += 2;            
            }

            db.SaveChanges();
        }

        private static void UpdateGroupActiveStatuses(KidsManagementDbContext db)
        {
            var groups = db.Groups.ToList();
            var counter = 0;
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].ActiveStatus = (GroupActiveStatus)counter;
                counter++;
                if (counter == 4)
                    counter = 0;
            }

            db.SaveChanges();
        }

        private static void UpdateTeachersStatuses(KidsManagementDbContext db)
        {
            var teachers = db.Teachers
                           .Include(t => t.Groups)
                           .ToArray();
            foreach (var teacher in teachers)
            {
                if (teacher.Groups.Count == 0)
                    teacher.Status = TeacherStatus.Initial;
                else
                {
                    teacher.Status = TeacherStatus.Active;
                }
            }
            db.SaveChanges();
        }

        private static void testGettingPicString()
        {
            var full = "http://res.cloudinary.com/svilenpavlov/image/upload/v1601636477/tf9xtcd5jwkxkzaqnzf3.jpg";
            Uri uri = new UriBuilder(full).Uri;
            string versionString = uri.Segments[5]; // v1/
            var test = versionString.Remove(versionString.IndexOf('.'));
        }

        private static void UpdateParentsStatuses(KidsManagementDbContext db)
        {
            var parents = db.Parents
                            .Include(p => p.Children)
                            .ThenInclude(sp => sp.Student)
                            .ToArray();
            foreach (var parent in parents)
            {
                //if (parent.Id == 72)
                //    Console.WriteLine();
                if (parent.Children.Count == 0)
                {
                    parent.Status = ParentStatus.Initial;
                }
                else
                {
                    if (parent.Children.Any(s => s.Student.Status == StudentStatus.Active))
                    {
                        parent.Status = ParentStatus.Active;
                    }
                    else if (parent.Children.All(s => s.Student.Status == StudentStatus.Quit))
                    {
                        parent.Status = ParentStatus.Quit;
                    }
                    else if (parent.Children.All(s => s.Student.Status == StudentStatus.Inactive))
                    {
                        parent.Status = ParentStatus.Inactive;
                    }
                    else
                    {
                        parent.Status = ParentStatus.Initial; //parent has at least 1 child that can still become active
                    }
                }
            }

            db.SaveChanges();
        }

        private static void UpdateStudentGrades(KidsManagementDbContext db)
        {
            var students = db.Students
               .ToArray();

            foreach (var student in students)
            {
                var age = student.Age;
                if (age >= 3 && age <= 19) student.Grade = (GradeLevel)(age - 3);
                else student.Grade = GradeLevel.Other;
            }

            db.SaveChanges();
        }

        private static void UpdateStudentStatuses(KidsManagementDbContext db)
        {
            var students = db.Students
               .ToArray();

            foreach (var student in students)
            {
                if (student.GroupId == null)
                {
                    student.Status = StudentStatus.Waiting;
                }
                else
                {
                    student.Status = StudentStatus.Active;
                }

            }

            db.SaveChanges();
        }
        private static void UpdateGroupAgeGroups(KidsManagementDbContext db)
        {
            var groups = db.Groups
                            .ToArray();

            foreach (var grp in groups)
            {
                grp.AgeGroup = AgeGroup.Preschool;
            }

            db.SaveChanges();
        }


        private static void UpdateGroupStatuses(KidsManagementDbContext db)
        {
            var groups = db.Groups
                .Include(x => x.Students)
                .ToArray();

            foreach (var grp in groups)
            {
                int count = grp.Students.Count();
                int max = grp.MaxStudents;
                if (count == 0)
                {
                    grp.Status = GroupStatus.Empty;
                }
                else if (count < max)
                {
                    grp.Status = GroupStatus.NotFull;
                }
                else if (count == max)
                {
                    grp.Status = GroupStatus.Full;
                }
                else if (count > max)
                {
                    grp.Status = GroupStatus.OverLimit;
                }
            }

            //db.SaveChanges();

        }

        private static void SeedRoles(KidsManagementDbContext db)
        {
            var roles = new List<ApplicationRole>()
            {
            new ApplicationRole("Admin"),
            new ApplicationRole("Teacher"),
            new ApplicationRole("Student"),
            new ApplicationRole("Manager")
            };
            db.Roles.AddRange(roles);
            //db.SaveChanges();

        }

        private static void AddStudentsToGroup(KidsManagementDbContext db)
        {
            int amountOfStudents = 10;
            var students = db.Students
                .Where(s => s.GroupId == null)
                .Take(amountOfStudents)
                .ToArray();

            int amountOfGroups = 2;
            var groups = db.Groups.
                Where(g => g.MaxStudents <= g.Students.Count)
                .Take(amountOfGroups)
                .ToArray();

            for (int i = 0; i < 10; i++)
            {
                var student = students[i];
                if (i < amountOfStudents / 2)
                {
                    student.GroupId = groups[0].Id;

                }
                else
                {
                    student.GroupId = groups[1].Id;
                }
            }

            // db.SaveChanges();
        }

        private static void SeedStudents(KidsManagementDbContext db)
        {
            var students = new List<Student>();
            var startDate = DateTime.Now.AddYears(-6);
            var gender = Gender.Male;
            for (int i = 0; i < 100; i++)
            {

                int ageCounter = 5;
                var student = new Student
                {
                    FirstName = $"Student {i}",
                    MiddleName = "Ivanov",
                    LastName = "Petrov",
                    Age = ageCounter,
                    BirthDate = startDate,
                    Gender = gender,
                    Grade = i > 50 ? GradeLevel.FirstGroup : GradeLevel.GradeOne,
                    Status = StudentStatus.Initial,
                };
                students.Add(student);

                ageCounter = ageCounter >= 10 ? 5 : ageCounter++;
                gender = gender == Gender.Male ? Gender.Female : Gender.Female;
            }

            db.Students.AddRange(students);
            //  db.SaveChanges();
        }

        private static void SeedGroups(KidsManagementDbContext db)
        {
            var groups = new List<Group>();
            var startDate = DateTime.Now;
            var level = db.Levels.FirstOrDefault();
            var teachers = db.Teachers.Take(10).ToList();
            for (int i = 0; i < 10; i++)
            {
                var teacher = teachers[i];
                var group = new Group
                {
                    Teacher = teacher,
                    StartTime = new TimeSpan(17, 30, 0),
                    Name = $"{teacher.FirstName} {"MON"} {new TimeSpan(17, 30, 0)}",
                    AgeGroup = AgeGroup.Preschool,
                    CurrentLessonNumber = 1,
                    DayOfWeek = DayOfWeek.Monday,
                    Duration = new TimeSpan(1, 45, 0),
                    StartDate = startDate.Date,
                    EndDate = startDate.AddDays(16 * 7),
                    EndTime = new TimeSpan(19, 00, 0),
                    Level = level,

                };
                startDate.AddDays(1);

                groups.Add(group);
            }
            db.Groups.AddRange(groups);
            // db.SaveChanges();
        }

        private static void SeedLevels(KidsManagementDbContext db)
        {
            string smarty = "SMARTY";
            List<Level> levels = new List<Level>();
            for (int i = 0; i < smarty.Length; i++)
            {
                var level = new Level
                {
                    Name = smarty[i].ToString(),
                    Description = "Level " + (i + 1),
                    StudyMaterialsDescription = "textbook,notebook,map,abacus"
                };
                levels.Add(level);
            }

            db.Levels.AddRange(levels);
            //db.SaveChanges();
        }

        private static void SeedParentsTeachersAdmins(KidsManagementDbContext db)
        {
            var admin = new Admin
            {
                FirstName = "svilen",
                LastName = "pavlov",
                Gender = Gender.Male,
                HireDate = DateTime.Now,
                Salary = 1000m
            };

            List<Parent> parents = new List<Parent>();
            List<Teacher> teachers = new List<Teacher>();
            for (int i = 0; i < 100; i++)
            {
                var parent = new Parent
                {
                    FirstName = "Parent " + i,
                    LastName = "Ivanova",
                    Gender = Gender.Female,
                    PhoneNumber = "0888 888 888",
                    Email = "abv@abv.bg"

                };
                parents.Add(parent);

                var teacher = new Teacher
                {
                    FirstName = "Teacher " + (i + 100),
                    LastName = "Tomas",
                    Gender = Gender.Female,
                    HiringDate = DateTime.Now,
                    PhoneNumber = "0888 888 888",
                    Salary = 1000m
                };
                teachers.Add(teacher);
            }

            db.Admins.Add(admin);
            db.Parents.AddRange(parents);
            db.Teachers.AddRange(teachers);
            //db.SaveChanges();
        }
    }
}
