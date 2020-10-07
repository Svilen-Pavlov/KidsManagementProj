using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.ViewModels.Levels;
using Microsoft.EntityFrameworkCore;
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

        public async Task EditLevelsOfTeacher(EditTeacherLevelsViewModel model)
        {
            var levelsForDeletion = this.db.LevelTeachers.Where(x => x.TeacherId == model.TeacherId).ToList();
            this.db.LevelTeachers.RemoveRange(levelsForDeletion);

            var teacher = await this.db.Teachers
                .Include(t => t.QualifiedLevels)
                .FirstOrDefaultAsync(x => x.Id == model.TeacherId);

            var newLevelsIds = model.Levels.Where(x => x.Selected).Select(x => x.Id).ToArray();
            var newQualifiedLevels = this.db.Levels
                .Where(x => newLevelsIds.Contains(x.Id))
                .Select(ql => new LevelTeacher { Level = ql })
                .ToArray();
            foreach (var item in newQualifiedLevels)
            {
                teacher.QualifiedLevels.Add(item);
            }

            await this.db.SaveChangesAsync();
        }

        public IEnumerable<LevelSelectionViewModel> GetAllForSelection()
        {
            var levels = this.db.Levels.Select(l => new LevelSelectionViewModel
            {
                Id = l.Id,
                Name = l.Name,
            })
            .OrderBy(x => x.Id)
            .ToArray();

            var list = new List<LevelSelectionViewModel>(levels);

            return list;
        }

        public EditTeacherLevelsViewModel GetLevelsForEdit(int teacherId)
        {
            var allLevelsIds = this.db.LevelTeachers
               .Where(lt => lt.TeacherId == teacherId)
               .Select(x => x.LevelId)
               .ToList();
            var levels = this.db.Levels.Select(l => new LevelSelectionViewModel
            {
                Id = l.Id,
                Name = l.Name,
                Selected = allLevelsIds.Contains(l.Id)
            })
            .OrderBy(x => x.Id)
            .ToArray(); ;



            var model = new EditTeacherLevelsViewModel { Levels = levels };

            return model;
        }
    }
}
