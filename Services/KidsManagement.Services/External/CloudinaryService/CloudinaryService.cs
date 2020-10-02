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
        private readonly CloudinaryDotNet.Cloudinary cloudinary;
        public CloudinaryService(CloudinaryDotNet.Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploadPicASync(IFormFile file, string existingURI)
        {
            string picId = null;
            if (existingURI != null)
            {
                string idPart = new UriBuilder(existingURI).Uri.Segments[5];
                picId = idPart.Remove(idPart.IndexOf('.'));
            }

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
                    PublicId= picId,
                    Invalidate=true, //overwrites CDN cache within minutes
                    Overwrite = true,
                };
                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                return uploadResult.Uri.ToString();
            }
        }
    }
}
