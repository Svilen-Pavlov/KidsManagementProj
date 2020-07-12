using KidsManagement.Data;
using KidsManagement.ViewModels.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KidsManagement.Services.Levels
{
    public class LevelsService : ILevelsService
    {
        private readonly KidsManagementDbContext db;

        public LevelsService(KidsManagementDbContext db)
        {
            this.db = db;
        }

      

        public IEnumerable<LevelSelectionViewModel> GetAllForSelection()
        {
            var levels = this.db.Levels.Select(l => new LevelSelectionViewModel
            {
                Id = l.Id,
                Name = l.Name,
            }).OrderBy(x => x.Name)
            .ToArray(); 

            var list = new List<LevelSelectionViewModel>(levels);

            return list;
        }
    }
}
