using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Data.Models.Enums
{
    public enum ParentStatus
    {
        Initial = 0, //no active students
        Active = 1, //at least 1 active student
        Inactive = 2, // all nonquit are inactive
        Quit = 3 //All students "Quit"
    }
}
