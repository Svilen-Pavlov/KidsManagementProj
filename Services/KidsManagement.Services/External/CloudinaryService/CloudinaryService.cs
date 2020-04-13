using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace KidsManagement.Services.External.CloudinaryService
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;
        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploadProfilePicASync(IFormFile file)
        {
            byte[] destinationImg;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                destinationImg = memoryStream.ToArray();
            }

            using (var destinationStream = new MemoryStream(destinationImg))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, destinationStream),
                    Overwrite = true,
                };
                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                return uploadResult.Uri.ToString();
            }
        }
    }
}
