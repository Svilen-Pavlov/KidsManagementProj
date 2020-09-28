using KidsManagement.ViewModels.Parents;
using KidsManagement.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services.Parents
{
    public interface IParentsService
    {
        AllParentsDetailsViewModel GetAll();
        AllParentsDetailsViewModel GetAll(int studentId);
        IEnumerable<ParentsSelectionViewModel> GetAllForSelection(int studentId);

        Task<bool> Exists(int parentId);

        Task<ParentsDetailsViewModel> FindById(int parentId);

        Task<int> CreateParent(CreateParentInputModel model, string userAdminId);

        Task<int> Delete(int parentId);

        Task<AllStudentsDetailsViewModel> GetNonQuitStudents(int parentId);
    }
}
