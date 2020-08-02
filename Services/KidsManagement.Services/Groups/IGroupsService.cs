using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Groups;
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

        Task RemoveStudent(int studentId, int groupId);

        Task<bool> GroupExists(int groupId);

        AllGroupsDetailsViewModel GetAll();
        AllGroupsOfTeacherViewModel GetAllByTeacher(int teacherId);

        Task<bool> GroupIsFull(int groupId);

        IEnumerable<GroupSelectionViewModel> GetAllForSelection(int teacherId);

        IEnumerable<GroupSelectionViewModel> GetAllForSelection(bool includingGroupsWithAssignedTeacher);
        void ChangeTeacher(int newTeacherId, int groupId);  //teacher service or here?
    }
}
