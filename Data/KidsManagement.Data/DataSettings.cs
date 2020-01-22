using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Data
{
    public class DataSettings
    {
        static readonly string dbName="KidsManagement";
        public static string ConnString = $"Server=SVILEN-PC\\SQLEXPRESS; Database={dbName}; Integrated Security=True;"; 
    }
}
