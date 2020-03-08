using KidsManagement.ViewModels.Group;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Services.Groups
{
    public interface IGroupsService
    {
        void Create(GroupCreateInputModel model);
    }
}
