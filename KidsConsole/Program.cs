﻿using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using KidsManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using KidsManagement.Services.Groups;

namespace KidsManagementConsole
{
    public class Program
    {
        public static void Main()
        {
            //var db = new KidsManagementDbContext();
            //var gs = new GroupsService(db);
            //var result = gs.GetAll();
            //Console.WriteLine(gs.GetAll().ToString());

            //db.Database.Migrate();
            Console.WriteLine();
            //SeedParentsTeachersAdmins(parents, teachers, db);
            //SeedLevels(db);
            //SeedGroups(db);


        }

        private static void SeedGroups(KidsManagementDbContext db)
        {
            var groups = new List<Group>();
            // var day = DayOfWeek.Monday;
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
                    CreatedOn=DateTime.UtcNow,
                    
                };
                startDate.AddDays(1);

                groups.Add(group);
            }
            db.Groups.AddRange(groups);
            db.SaveChanges();
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
            // db.SaveChanges();
        }

        private static void SeedParentsTeachersAdmins(KidsManagementDbContext db)
        {
            var admin = new Admin
            {
                FirstName = "svilen",
                LastName = "pavlov",
                Gender = Gender.Male,
                HireDate = DateTime.Now,
                Salary = 1360m
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
                    Salary = 1000m
                };
                teachers.Add(teacher);
            }
            
            db.Admins.Add(admin);
            db.Parents.AddRange(parents);
            db.Teachers.AddRange(teachers);
            // db.SaveChanges();
        }
    }
}
