using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Notes
{
    public class NotesSelectionViewModel
    {
        public int Id { get; set; }

        public string AdminName { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }

        public bool Selected { get; set; } //checkbox menus
    }
}
