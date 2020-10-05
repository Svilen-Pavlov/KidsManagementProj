using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Levels
{
    public class EditTeacherLevelsViewModel
    {
        
        public int TeacherId { get; set; }
        public IList<LevelSelectionViewModel> Levels { get; set; }
    } 
        
}
