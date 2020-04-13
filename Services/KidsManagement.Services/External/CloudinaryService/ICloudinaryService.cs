using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KidsManagement.Services.External.CloudinaryService
{
   public interface ICloudinaryService
    {
        Task<string> UploadProfilePicASync(IFormFile file);
    }
}
