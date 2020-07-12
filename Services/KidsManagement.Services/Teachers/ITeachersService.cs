using KidsManagement.ViewModels.Teachers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services.Teachers
{
    public interface ITeachersService
    {
        TeacherDetailsViewModel FindById (int teacherId);

        Task<int> CreateTeacher(CreateTeacherInputModel model);

        Task<bool> TeacherExists(int teacherId);

        AllTeachersListViewModel GetAll();

        IEnumerable<TeacherSelectionViewModel> GetAllDropDown();
    }
}
