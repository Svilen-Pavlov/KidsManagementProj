using KidsManagement.ViewModels.Teachers;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Services.Teachers
{
    public interface ITeachersService
    {
        TeacherDetailsViewModel FindById (int teacherId);

        int CreateTeacher(TeacherCreateInputModel model);

        bool TeacherExists(int teacherId);

        AllTeachersListViewModel GetAll();

        IEnumerable<TeacherDropDownViewModel> GetAllDropDown();
    }
}
