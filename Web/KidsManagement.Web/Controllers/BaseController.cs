using KidsManagement.Services.Groups;
using KidsManagement.Services.Levels;
using KidsManagement.Services.Parents;
using KidsManagement.Services.Students;
using KidsManagement.Services.Teachers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KidsManagement.Web.Controllers
{
    public class BaseController:Controller
    {
        protected readonly IGroupsService groupsService;
        protected readonly ITeachersService teachersService;
        protected readonly IStudentsService studentsService;
        protected readonly IParentsService parentsService;
        //private readonly ILevelsService levelsService;
        public BaseController(IGroupsService groupsService, ITeachersService teachersService, IStudentsService studentsService, IParentsService parentsService/*, ILevelsService levelsService*/)
        {
            this.groupsService = groupsService;
            this.teachersService = teachersService;
            this.studentsService = studentsService;
            this.parentsService = parentsService;
            //this.levelsService = levelsService;
        }
        public async Task<int> CheckGroupId(object groupIdNullable)
        {
            if (groupIdNullable == null || (groupIdNullable is int) == false)
                throw new Exception(); //todo invalid userId Exception

            int groupId = (int)groupIdNullable;
            if (await this.groupsService.GroupExists(groupId) == false)
                throw new Exception(); //todo teacher does not exist Exception

            return groupId;
        }
        public async Task<int> CheckTeacherId(object teacherIdnullabe)
        {
            if (teacherIdnullabe == null || (teacherIdnullabe is int) == false)
                throw new Exception(); //todo invalid userId Exception

            int teacherId = (int)teacherIdnullabe;
            if (await this.teachersService.TeacherExists(teacherId) == false)
                throw new Exception(); //todo teacher does not exist Exception

            return teacherId;
        }

        public async Task<int> CheckStudentId(object studentIdNullable)
        {
            if (studentIdNullable == null || (studentIdNullable is int) == false)
                throw new Exception(); //todo invalid userId Exception

            int studentId = (int)studentIdNullable;
            if (await this.studentsService.Exists(studentId) == false)
                throw new Exception(); //todo student does not exist Exception

            return studentId;
        }

        public async Task<int> CheckParentId(object parentIdNullable)
        {
            if (parentIdNullable == null || (parentIdNullable is int) == false)
                throw new Exception(); //todo invalid userId Exception

            int parentId = (int)parentIdNullable;

            if (await this.parentsService.Exists(parentId) == false)
                throw new Exception(); //todo parent does not exist Exception

            return parentId;
        }
    }
}
