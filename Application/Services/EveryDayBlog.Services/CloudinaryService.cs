using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet;
using System.IO;
using EveryDayBlog.Data.Models;
using CloudinaryDotNet.Actions;

namespace EveryDayBlog.Services
{
    public class CloudinaryService : ICloudinaryService
    {

        private readonly Cloudinary cloudinaryUtility;

        public CloudinaryService(Cloudinary cloudinaryUtility)
        {
            this.cloudinaryUtility = cloudinaryUtility;
        }

        public string UploudPicture(Image image)
        {
            UploadResult uploadResult;

            using (var ms = new MemoryStream(image.ImageByte))
            {
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    Folder = "Users",
                    File = new FileDescription(image.ImageTitle, ms),
                };

                uploadResult = this.cloudinaryUtility.Upload(uploadParams);
            }

            return uploadResult?.SecureUri.AbsoluteUri;
        }
    }
}
