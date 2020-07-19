using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.ViewModels.Parents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KidsManagement.Services.Parents
{
    public class ParentsService : IParentsService
    {
        private readonly KidsManagementDbContext db;

        public ParentsService(KidsManagementDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<ParentsSelectionViewModel> GetAllForSelection(int studentId)
        {
            var currentParentsIds = this.db.StudentParents.Where(x => x.StudentId == studentId).Select(x => x.ParentId).ToArray();
            var list = this.db.Parents.Select(x =>
                new ParentsSelectionViewModel
                {
                    Id = x.Id,
                    Name = x.FullName,
                    Selected = currentParentsIds.Contains(x.Id) ? true : false
                })
                .ToArray()
                .OrderBy(x => x.Name)
                .ToList();
            return list;
        }
       

    }
}
