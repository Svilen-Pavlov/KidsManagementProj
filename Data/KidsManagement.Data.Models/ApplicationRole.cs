﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Data.Models
{
    public class ApplicationRole:IdentityRole
    {
        public ApplicationRole(string name)
           : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
