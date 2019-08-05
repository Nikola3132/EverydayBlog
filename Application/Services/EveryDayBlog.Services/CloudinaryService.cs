namespace EveryDayBlog.Services
{
    using System;
    using System.IO;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using EveryDayBlog.Web.Infrastructure.Models;

    public class CloudinaryService : ICloudinaryService
    {

        private readonly Cloudinary cloudinaryUtility;

        public CloudinaryService(Cloudinary cloudinaryUtility)
        {
            this.cloudinaryUtility = cloudinaryUtility;
        }

        public string UploudPicture(ImageInputModel image, string folderName)
        {
            UploadResult uploadResult;

            using (var ms = new MemoryStream(image.ImageByte))
            {
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    Folder = folderName,
                    File = new FileDescription(image.ImageTitle, ms),
                    PublicId = Guid.NewGuid().ToString(),
                    Invalidate = true,
                };

                uploadResult = this.cloudinaryUtility.Upload(uploadParams);
            }

            return uploadResult?.SecureUri.AbsoluteUri;
        }
    }
}
