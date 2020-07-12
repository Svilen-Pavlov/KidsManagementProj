using KidsManagement.ViewModels.Levels;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Services.Levels
{
    public interface ILevelsService
    {
        IEnumerable<LevelSelectionViewModel> GetAllForSelection();
    }
}
