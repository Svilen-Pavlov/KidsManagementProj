using KidsManagement.Data;
using KidsManagement.ViewModels.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services.Levels
{
    public class LevelsService : ILevelsService
    {
        private readonly KidsManagementDbContext db;

        public LevelsService(KidsManagementDbContext db)
        {
            this.db = db;
        }

        public Task EditLevelsOfTeacher(EditTeacherLevelsViewModel model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LevelSelectionViewModel> GetAllForSelection()
        {
            var levels = this.db.Levels.Select(l => new LevelSelectionViewModel
            {
                Id = l.Id,
                Name = l.Name,
            }).OrderBy(x => x.Id)
            .ToArray(); 

            var list = new List<LevelSelectionViewModel>(levels);

            return list;
        }

        public EditTeacherLevelsViewModel GetLevelsForEdit(int teacherId)
        {
            var teacherLevels = this.db.LevelTeachers
                .Where(lt => lt.TeacherId == teacherId)
                .Select(x => new LevelSelectionViewModel
                {
                    Id=x.LevelId,
                    Name=x.Level.Name,
                    Selected=true
                })
                .ToList();
            var model = new EditTeacherLevelsViewModel { Levels = teacherLevels};

            return model;
        }
    }
}
