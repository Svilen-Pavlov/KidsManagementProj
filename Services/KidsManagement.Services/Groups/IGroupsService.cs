using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Groups;
using KidsManagement.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services.Groups
{
    public interface IGroupsService
    {
        Task<int> CreateGroup(CreateGroupInputModel model);

        GroupDetailsViewModel FindById(int id);

        Task AddStudent(int studentId, int groupId);

        Task RemoveStudent(int studentId);

        Task<bool> GroupExists(int groupId);

        AllGroupsDetailsViewModel GetAll();
        AllGroupsDetailsViewModel GetAll(int teacherId);
        AllGroupsOfTeacherViewModel GetTeacherGroups(int teacherId);
        AllGroupsOfTeacherViewModel GetActiveGroups(int teacherId);

        //Task<bool> GroupIsFull(int groupId);

        Task<IEnumerable<SingleGroupDetailsViewModel>> GetVacantGroupsWithProperAge(int studentId);

        IEnumerable<GroupSelectionViewModel> GetAllForSelection(int teacherId);

        IEnumerable<GroupSelectionViewModel> GetAllForSelection(bool includingGroupsWithAssignedTeacher);
        void ChangeTeacher(int newTeacherId, int groupId);
    }
}
