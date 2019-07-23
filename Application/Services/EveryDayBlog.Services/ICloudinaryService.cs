using EveryDayBlog.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EveryDayBlog.Services
{
    public interface ICloudinaryService
    {
       string UploudPicture(Image image);
    }
}
