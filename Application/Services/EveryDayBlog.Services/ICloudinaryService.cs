using EveryDayBlog.Data.Models;
using EveryDayBlog.Web.ViewModels.Images.InputModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EveryDayBlog.Services
{
    public interface ICloudinaryService
    {
       string UploudPicture(ImageInputModel image);

    }
}
