using KidsManagement.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Services.Students
{
    public interface IStudentsService
    {
        int CreateStudent(CreateStudentInputModel model);

        bool StudentExists(int StudentId);


    }
}
