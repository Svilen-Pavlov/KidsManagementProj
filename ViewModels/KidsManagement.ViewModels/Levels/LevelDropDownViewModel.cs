using KidsManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Levels
{
    public class LevelSelectionViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public bool Selected { get; set; } //for checkbox forms specifically

    }
}
