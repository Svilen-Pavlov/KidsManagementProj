using KidsManagement.Data;
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
        public IEnumerable<ParentsSelectionViewModel> GetAllForSelection()
        {
            var list = this.db.Parents.Select(x =>
                new ParentsSelectionViewModel
                {
                    Id = x.Id,
                    Name = x.FullName
                }).ToArray();

            return list;
        }

       
    }
}
