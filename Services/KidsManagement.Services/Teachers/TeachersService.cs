﻿using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.ViewModels.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KidsManagement.Services.Teachers
{
    public class TeachersService : ITeachersService
    {
        private readonly KidsManagementDbContext db;

        public TeachersService(KidsManagementDbContext db)
        {
            this.db = db;
        }

        public int CreateTeacher(TeacherCreateInputModel model) //idk if this populates teacherlevels correctly
        {
            var levelNames = model.QualifiedLevels.ToArray();
            var levelsIds = this.db.Levels.Where(x => levelNames.Contains(x.Name)).Select(x=>x.Id).ToArray();
            var levelTeacherList = new List<LevelTeacher>();
            var teacher = new Teacher
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                HiringDate = model.HiringDate,
                DismissalDate = model.DismissalDate,
                Salary = model.Salary,
            };
            this.db.Teachers.Add(teacher);

            foreach (var levelId in levelsIds)
            {
                var levelTeacher = new LevelTeacher
                {
                    LevelId = levelId,
                    TeacherId = teacher.Id
                };
                levelTeacherList.Add(levelTeacher);

            }

            this.db.LevelTeachers.AddRange(levelTeacherList);
            this.db.SaveChanges();

            return teacher.Id;
        }


        public TeacherDetailsViewModel FindById(int teacherId)
        {
            var teacher = this.db.Teachers.FirstOrDefault(x => x.Id == teacherId);
            var levelsIds = this.db.LevelTeachers.Where(x => x.TeacherId == teacherId).Select(x=>x.LevelId).ToArray();
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

        public bool TeacherExists(int teacherId)
        {
            return this.db.Teachers.Any(x => x.Id == teacherId);
        }
    }
}