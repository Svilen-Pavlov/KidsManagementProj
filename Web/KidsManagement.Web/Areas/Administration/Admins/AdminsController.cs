using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KidsManagement.Web.Areas.Administration.Admins
{
    [Authorize(Roles ="Admin1")]
    public class AdminsController:Controller
    {
        public IActionResult Index()
        {
            return Json("Ok:1");
        }
    }
}
