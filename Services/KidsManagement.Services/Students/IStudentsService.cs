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

        Task<bool> StudentExists(int studentId);

        Task AssignStudentToGroup(int studentId, int groupId);

        Task<StudentDetailsViewModel> FindById(int studentId);
    }
}
