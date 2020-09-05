using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Parents
{
    public class ParentsSelectionViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public bool Selected { get; set; } //checkbox menus
    }
}
