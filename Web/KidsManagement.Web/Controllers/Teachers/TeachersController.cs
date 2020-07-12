using KidsManagement.Services.Levels;
using KidsManagement.Services.Teachers;
using KidsManagement.ViewModels.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KidsManagement.Web.Controllers.Teachers
{
    [Authorize(Roles = "Admin")]
    public class TeachersController :Controller
    {
        private readonly ITeachersService teachersService;
        private readonly ILevelsService levelsService;

        public TeachersController(ITeachersService teachersService, ILevelsService levelsService)
        {
            this.teachersService = teachersService;
            this.levelsService = levelsService;
        }

        public IActionResult Create()
        {
            var levelsList = this.levelsService.GetAllForSelection();
            var model = new TeacherCreateInputModel()
            {
                Levels = levelsList.ToList()
            };
            return this.View(model);
        }
    }
}
