using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
using KidsManagement.Services.External.CloudinaryService;
using KidsManagement.ViewModels.Notes;
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

        public async Task<int> CreateParent(CreateEditParentInputModel model, string userAdminId)
        {
           
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
                ProfilePicURI = model.ProfileImage == null ? Const.defaultProfPicURL : await cloudinaryService.UploadPicASync(model.ProfileImage, null),

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
            var notes = this.db.Notes
                .Include(n => n.Admin)
                .Where(n => n.ParentId == parent.Id).ToArray();

            var model = new ParentsDetailsViewModel
            {
                Id = parent.Id,
                FirstName = parent.FirstName,
                LastName = parent.LastName,
                Gender = parent.Gender,
                PhoneNumber = parent.PhoneNumber,
                AlternativePhoneNumber = parent.AlternativePhoneNumber,
                Email = parent.Email,
                AlternativeEmail = parent.AlternativeEmail,
                Status = parent.Status,
                ProfilePicURI = parent.ProfilePicURI,
                Children = children.Select(p => new StudentSelectionViewModel
                {
                    Id = p.Id,
                    Name = p.FullName
                }).ToList(),
                AdminNotes = notes.Select(n => new NotesSelectionViewModel
                {
                    AdminName = n.Admin.FullName,
                    Content = n.Content,
                    Date = n.Date,
                    Id = n.Id
                }).ToList()

            };

            return model;
        }

        public IEnumerable<ParentsSelectionViewModel> GetAllForSelection(int studentId)
        {
            var currentParentsIds = this.db.StudentParents.Where(x => x.StudentId == studentId).Select(x => x.ParentId).ToArray(); //2 db operations - TODO optimize
            var list = this.db.Parents
                .Where(x => currentParentsIds.Contains(x.Id) == false)
                .Select(x =>
                new ParentsSelectionViewModel
                {
                    Id = x.Id,
                    Name = x.FullName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Selected = false
                })
                .ToArray()
                .OrderByDescending(x => x.Selected)
                .ThenBy(x => x.Name)
                .ToList();
            return list;
        }

        public AllParentsDetailsViewModel GetAll()
        {
            return this.GetAll(0);
        }

        public AllParentsDetailsViewModel GetAll(int studentId)
        {
            var parentsRaw = this.db.Parents
                .Where(p => p.Status != ParentStatus.Quit) 
                .Include(p => p.Children)
                .Where(p => (studentId != 0) ? p.Children.Any(c => c.ParentId == p.Id) : true)
                .ToArray();

            var parents = parentsRaw
              .Select(parent => new AllSingleParentsViewModel
              {
                  Id = parent.Id,
                  FullName = parent.FullName,
                  Gender = parent.Gender,
                  StudentsCount = parent.Children.Count(),
                  Email = parent.Email,
                  PhoneNumber = parent.PhoneNumber
              })
              .ToArray()
              .OrderBy(x => x.FullName)
              .ToArray();


            var parentsList = new List<AllSingleParentsViewModel>(parents);

            var model = new AllParentsDetailsViewModel() { Parents = parentsList };

            return model;
        }

        public async Task<int> Delete(int parentId)
        {
            var parent = await this.db.Parents
                .Include(p=>p.Children)
                .ThenInclude(sp=>sp.Student)
                .FirstOrDefaultAsync(s => s.Id == parentId);

            if (parent.Children.All(sp=>sp.Student.Status == StudentStatus.Quit) || parent.Children.Count==0)
            parent.Status = ParentStatus.Quit;


            return this.db.SaveChangesAsync().Result;
        }

        public async Task<AllStudentsDetailsViewModel> GetNonQuitStudents(int parentId)
        {
            var parent = await this.db.Parents
                .Include(p => p.Children)
                .ThenInclude(sp => sp.Student)
                .FirstOrDefaultAsync(s => s.Id == parentId);

            var students = parent.Children
                .Where(sp => sp.Student.Status != StudentStatus.Quit)
                .Select(student => new AllSingleStudentsViewModel
                {
                    Id = student.Student.Id,
                    FullName = student.Student.FullName,
                    Age = student.Student.Age,
                    Gender = student.Student.Gender,
                    Status=student.Student.Status
                })
                .ToArray();

            var studentsList = new List<AllSingleStudentsViewModel>(students);

            var model = new AllStudentsDetailsViewModel() { Students = studentsList };

            return model;
        }

        public async Task<CreateEditParentInputModel> GetInfoForEdit(int parentId)
        {
            var parent = await this.db.Parents
                .FirstOrDefaultAsync(x => x.Id == parentId);

            var model = new CreateEditParentInputModel
            {
                FirstName = parent.FirstName,
                LastName = parent.LastName,
                Gender = parent.Gender,
                Email=parent.Email,
                AlternativeEmail=parent.AlternativeEmail,
                PhoneNumber=parent.PhoneNumber,
                AlternativePhoneNumber=parent.AlternativePhoneNumber,
                ProfilePicURI = parent.ProfilePicURI,
            };

            return model;
        }

        public async Task EditInfo(CreateEditParentInputModel model)
        {
            var parent = await this.db.Parents.FirstOrDefaultAsync(x => x.Id == model.Id);

            parent.FirstName = model.FirstName;
            parent.LastName = model.LastName;
            parent.Gender = model.Gender;
            parent.Email = model.Email;
            parent.AlternativeEmail = model.AlternativeEmail;
            parent.PhoneNumber = model.PhoneNumber;
            parent.AlternativePhoneNumber = model.AlternativePhoneNumber;
            parent.ProfilePicURI = model.ProfileImage == null ? Const.defaultProfPicURL : await cloudinaryService.UploadPicASync(model.ProfileImage, parent.ProfilePicURI);

            await this.db.SaveChangesAsync();
        }
    }
}
