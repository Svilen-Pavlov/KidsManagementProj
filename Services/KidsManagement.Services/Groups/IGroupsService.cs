using KidsManagement.Data.Models;
using KidsManagement.ViewModels.Groups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services.Groups
{
    public interface IGroupsService
    {
        Task<int> CreateGroup(GroupCreateInputModel model);

        GroupDetailsViewModel FindById(int id);

        Task AddStudent(int studentId, int groupId);

        Task RemoveStudent(int studentId, int groupId);

        Task<bool> GroupExists(int groupId);

        AllGroupsDetailsViewModel GetAll();

        Task<bool> GroupIsFull(int groupId);
    }
}
