using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.Services.External.CloudinaryService;
using KidsManagement.ViewModels.Parents;
using KidsManagement.ViewModels.Students;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services.Parents
{
    public class ParentsService : IParentsService
    {
        private readonly KidsManagementDbContext db;
        private readonly ICloudinaryService cloudinaryService;

        public ParentsService(KidsManagementDbContext db, ICloudinaryService cloudinaryService)
        {
            this.db = db;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<bool> Exists(int parentId) // with or w/o deleted?
        {
            return await this.db.Parents.AnyAsync(x => x.Id == parentId);
        }

        public async Task<int> CreateParent(CreateParentInputModel model, string userAdminId)
        {
            var pic = model.ProfileImage;
            var picURI = pic == null ? string.Empty : await this.cloudinaryService.UploadProfilePicASync(pic);

            var adminId = db.Admins.FirstOrDefault(a => a.ApplicationUserId == userAdminId).Id;

            var parent = new Parent
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                AlternativePhoneNumber = model.AlternativePhoneNumber,
                Email = model.Email,
                AlternativeEmail = model.AlternativeEmail,
                ProfilePicURI = picURI,
                
            };
            var initialNote = new Note()
            {
                AdminId = adminId, 
                Content = model.InitialAdminNote,
                Date = DateTime.Now,
            };

            parent.AdminNotes.Add(initialNote);

            await this.db.Parents.AddAsync(parent);
            await this.db.SaveChangesAsync();
            return parent.Id;
        }


        public async Task<ParentsDetailsViewModel> FindById(int parentId)
        {
            var parent = await this.db.Parents
                //.Include(s=>s.Students)
                //.ThenInclude(x=>x.Select(y=>y.Student))
                .FirstOrDefaultAsync(x => x.Id == parentId);
            var children = this.db.Students.Where(p => p.Parents.Any(sp => sp.ParentId == parent.Id)).ToArray();

            var model = new ParentsDetailsViewModel
            {
                Id = parent.Id,
                FirstName = parent.FirstName,
                LastName = parent.LastName,
                Gender = parent.Gender,
                PhoneNumber=parent.PhoneNumber,
                AlternativePhoneNumber=parent.AlternativePhoneNumber,
                Email=parent.Email,
                AlternativeEmail=parent.AlternativeEmail,
                ProfilePicURI = parent.ProfilePicURI,
                Children = children.Select(p => new StudentSelectionViewModel
                //Notes!
                {
                    Id = p.Id,
                    Name = p.FullName
                }).ToList()
            };

            return model;
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
