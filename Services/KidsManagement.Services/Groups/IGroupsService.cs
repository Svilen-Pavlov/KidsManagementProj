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

        void AddStudent(int studentId, int groupId);

        void RemoveStudent(int studentId, int groupId);

        bool GroupExists(int groupId);

        AllGroupsDetailsViewModel GetAll();
    }
}
