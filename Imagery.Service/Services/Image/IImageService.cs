﻿using Imagery.Service.ViewModels.Image;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.Services.Image
{
    public interface IImageService
    {
        Task<string> UploadProfilePicture(string username, IFormFile file);
        ExponentItemVM UploadItem(int id, ItemUploadVM itemUpload);
        List<ExponentItemVM> GetExhibitionItems(int id);
    }
}
