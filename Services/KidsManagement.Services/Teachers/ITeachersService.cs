﻿using KidsManagement.ViewModels.Groups;
using KidsManagement.ViewModels.Teachers;
using KidsManagement.ViewModels.Teachers.MyZoneModels;
using KidsManagement.ViewModels.Teachers.MyZoneModels.MySchedule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services.Teachers
{
    public interface ITeachersService
    {
        TeacherDetailsViewModel FindById (int teacherId);

        Task<int> CreateTeacher(CreateEditTeacherInputModel model);

        Task<bool> TeacherExists(int teacherId);

        AllTeachersListViewModel GetAll();

        IEnumerable<TeacherSelectionViewModel> GetAllForSelection();

        Task<bool> UserExists(string username);

        Task<int> AddGroups(AddGroupsToTeacherViewModel model);

        Task<int> GetBussinessIdByUserId(string userId);

        Task<CreateEditTeacherInputModel> GetInfoForEdit(int teacherId);
        Task EditInfo(CreateEditTeacherInputModel model);

        Task<int> Delete(int teacherId);

        TeacherMyZoneViewModel GetMyZoneInfo(int teacherId);

        ScheduleViewModel GetMySchedule(int teacherId, DateTime startdate, int marker);

        Task<int> AssignGroup(int teacherId, int groupId);

        Task<int> UnassignGroup(int groupId);

        Task<AllTeachersListViewModel> GetAllEligibleTeacherForGroup(int groupId);

    }
}
