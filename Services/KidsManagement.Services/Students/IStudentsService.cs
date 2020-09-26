using KidsManagement.ViewModels.Parents;
using KidsManagement.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services.Students
{
    public interface IStudentsService
    {
        Task<int> CreateStudent(CreateEditStudentInputModel model);

       
        Task<bool> Exists(int studentId);

        Task<StudentDetailsViewModel> FindById(int studentId);

        AllStudentsDetailsViewModel GetAll();
        AllStudentsDetailsViewModel GetAll(int teacherId);

        Task<CreateEditStudentInputModel> GetInfoForEdit(int studentId);
        Task EditInfo(CreateEditStudentInputModel model);

        Task AddParents(EditParentsInputModel model);

        Task<int> UnassignParent(int studentId, int parentId);

        Task<int> Delete(int studentId);
    }
}
