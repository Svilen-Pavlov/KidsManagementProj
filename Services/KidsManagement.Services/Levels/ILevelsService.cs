using KidsManagement.ViewModels.Levels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services.Levels
{
    public interface ILevelsService
    {
        IEnumerable<LevelSelectionViewModel> GetAllForSelection();

        EditTeacherLevelsViewModel GetLevelsForEdit(int teacherId);

        Task EditLevelsOfTeacher(EditTeacherLevelsViewModel model);
    }
}
