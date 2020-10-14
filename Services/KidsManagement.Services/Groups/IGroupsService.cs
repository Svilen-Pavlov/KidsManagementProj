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
        Task<int> CreateGroup(CreateEditGroupInputModel model);

        GroupDetailsViewModel FindById(int id);

        Task<bool> AddStudentToGroup(int groupId, int studentId);

        Task RemoveStudentFromGroup(int studentId);

        Task<bool> IsGroupFull(int groupId);

        Task<bool> GroupExists(int groupId);

        AllGroupsDetailsViewModel GetAll();
        AllGroupsDetailsViewModel GetAll(int teacherId);
        AllGroupsOfTeacherViewModel GetGroupsByTeacher(int teacherId);
        AllGroupsOfTeacherViewModel GetActiveGroupsByTeacher(int teacherId);
        Task<CreateEditGroupInputModel> GetInfoForEdit(int groupId);
        Task EditInfo(CreateEditGroupInputModel model);
        Task<int> Delete(int studentId);
        Task<IEnumerable<SingleGroupDetailsViewModel>> GetVacantGroupsWithProperAge(int studentId);

        IEnumerable<GroupSelectionViewModel> GetAllForSelection(int teacherId);

        IEnumerable<GroupSelectionViewModel> GetAllForSelection(bool includingGroupsWithAssignedTeacher);
        void ChangeTeacher(int newTeacherId, int groupId);

    }
}
