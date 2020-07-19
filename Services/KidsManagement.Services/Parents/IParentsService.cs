using KidsManagement.ViewModels.Parents;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Services.Parents
{
    public interface IParentsService
    {
        IEnumerable<ParentsSelectionViewModel> GetAllForSelection(int studentId);

    }
}
