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
        Task<int> CreateStudent(CreateStudentInputModel model);

        Task<bool> Exists(int studentId);

        Task<StudentDetailsViewModel> FindById(int studentId);

        AllStudentsDetailsViewModel GetAll();

        Task EditParents(EditParentsInputModel model);

    }
}
