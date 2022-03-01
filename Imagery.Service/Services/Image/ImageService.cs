using Imagery.Core.Models;
using Imagery.Repository.Repository;
using Imagery.Service.Helpers;
using Imagery.Service.Services.Exhbition;
using Imagery.Service.ViewModels.Image;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly UserManager<User> UserManager;
        private readonly IWebHostEnvironment HostEnviroment;
        private readonly IHttpContextAccessor ContextAccessor;
        private readonly IRepository<ExponentItem> ItemRepository;

        public ImageService(UserManager<User> userManager, IWebHostEnvironment hostEnviroment, IHttpContextAccessor contextAccessor, IRepository<ExponentItem> itemRepository)
        {
            UserManager = userManager;
            HostEnviroment = hostEnviroment;
            ContextAccessor = contextAccessor;
            ItemRepository = itemRepository;
        }

        public async Task<string> UploadProfilePicture(string username, IFormFile file)
        {
            var userExists = await UserManager.FindByNameAsync(username);

            if (userExists == null)
            {
                return "User doesn't exist!";
            }
            const string folder = "ProfilePictures";

            string profilePicture = EditImage(folder, file, userExists.ProfilePicture);

            if (string.IsNullOrEmpty(profilePicture))
            {
                return "Error, something went wrong try again!";
            }

            userExists.ProfilePicture = profilePicture;

            var result =  await UserManager.UpdateAsync(userExists);

            if (!result.Succeeded)
            {
                return "Error profile pic not updated!";
            }

            return profilePicture;
        }

        private string SaveImage(string folder, IFormFile file)
        {
            string rootPath = HostEnviroment.WebRootPath;

            string image = Path.GetFileNameWithoutExtension(file.FileName);
            string extensions = Path.GetExtension(file.FileName);
            image += Guid.NewGuid().ToString() + extensions;

            string imagePath = rootPath + $"\\{folder}\\" + image;

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            var url = $"{ContextAccessor.HttpContext.Request.Scheme}://{ContextAccessor.HttpContext.Request.Host}";
            var imageURL = Path.Combine(url, folder, image).Replace("\\", "/");

            return imageURL;
        }

        private void DeleteImage(string folder, string imagepath)
        {
            if (string.IsNullOrEmpty(imagepath))
            {
                return;
            }

            string imageName = Path.GetFileName(imagepath);
            string filePath = Path.Combine(HostEnviroment.WebRootPath, folder, imageName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private string EditImage(string folder, IFormFile image, string path)
        {
            DeleteImage(folder, path);

            return SaveImage(folder, image);
        }

        public List<ExponentItemVM> GetExhibitionItems(int id)
        {
            List<ExponentItemVM> items = ItemRepository.GetAll().Where(item => item.ExhibitionId == id).Select(item => new ExponentItemVM() 
            {
                Name = item.Name,
                Description = item.Description,
                Creator = item.Creator,
                Image = item.Image,
                Dimensions = item.Dimensions,
                Price = item.Price
            }).ToList();

            return items;
        }

        public  ExponentItemVM UploadItem(int id, ItemUploadVM itemUpload)
        {
            const string folder = "ExponentItems";

            string imagePath = EditImage(folder, itemUpload.Image, null);

            if (string.IsNullOrEmpty(imagePath))
            {
                return null;
            }

            var repoResponse = ItemRepository.Add(new ExponentItem()
            {
                Name = itemUpload.Name,
                Description = itemUpload.ImageDescription,
                Creator = itemUpload.Creator,
                Dimensions = itemUpload.Dimensions,
                ExhibitionId = id,
                Price = itemUpload.Price,
                Image = imagePath
            });

            if (!repoResponse.IsSuccess)
            {
                return null;
            }

            ExponentItemVM exponentItem = Mapper.MapExponentItem(repoResponse.Content);

            return exponentItem;
            
        }
    }
}
