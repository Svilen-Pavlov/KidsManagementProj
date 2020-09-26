﻿using KidsManagement.ViewModels.Parents;
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
        AllStudentsDetailsViewModel GetAll(int teacherId);

        Task<CreateStudentInputModel> GetInfoForEdit(int studentId);
        Task EditInfo(CreateStudentInputModel model);

        Task AddParents(EditParentsInputModel model);

        Task<int> UnassignParent(int studentId, int parentId);

        Task<int> Delete(int studentId);
    }
}
