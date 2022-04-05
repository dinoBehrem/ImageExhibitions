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
        private readonly IRepository<Dimensions> DimensionsRepository;
        private readonly IRepository<CollectionItem> CollectionRepository;

        public ImageService(UserManager<User> userManager, IWebHostEnvironment hostEnviroment, IHttpContextAccessor contextAccessor, IRepository<ExponentItem> itemRepository, IRepository<Dimensions> dimensionsRepository, IRepository<CollectionItem> collectionRepository)
        {
            UserManager = userManager;
            HostEnviroment = hostEnviroment;
            ContextAccessor = contextAccessor;
            ItemRepository = itemRepository;
            DimensionsRepository = dimensionsRepository;
            this.CollectionRepository = collectionRepository;
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

        private bool DeleteImage(string folder, string imagepath)
        {
            if (string.IsNullOrEmpty(imagepath))
            {
                return false;
            }

            string imageName = Path.GetFileName(imagepath);
            string filePath = Path.Combine(HostEnviroment.WebRootPath, folder, imageName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return true;
        }

        private string EditImage(string folder, IFormFile image, string path)
        {
            DeleteImage(folder, path);

            return SaveImage(folder, image);
        }

        public List<ExponentItemVM> GetExhibitionItems(int id)
        {
            List<ExponentItemVM> items = ItemRepository.Find(item => item.ExhibitionId == id).Select(item => new ExponentItemVM() 
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Creator = item.Creator,
                Image = item.Image,
                Dimensions = GetItemDimensions(item.Id),
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
                ExhibitionId = id,
                Image = imagePath
            });

            if (!repoResponse.IsSuccess)
            {
                return null;
            }

            ExponentItemVM exponentItem = Mapper.MapExponentItemVM(repoResponse.Content);
            exponentItem.Dimensions = new List<DimensionsVM>();

            return exponentItem;
            
        }

        private List<DimensionsVM> GetItemDimensions(int itemId)
        {
            List<DimensionsVM> dimensions = DimensionsRepository.Find(dimension => dimension.ExponentItemId == itemId).Select(dimension => new DimensionsVM()
            { 
                Dimension = dimension.Dimension,
                Price = dimension.Price,
                Id = dimension.Id
            }).ToList();

            return dimensions;
        }

        public DimensionsVM AddDimensions(int id, DimensionsVM dimensions)
        {
            var exponentItemExists = ItemRepository.GetSingleOrDefault(id);

            if (!exponentItemExists.IsSuccess)
            {
                return null;
            }


            var setDimensions = DimensionsRepository.Add(new Dimensions() { Dimension = dimensions.Dimension, Price = dimensions.Price, ExponentItemId = exponentItemExists.Content.Id });

            if (!setDimensions.IsSuccess)
            {
                return null;
            }

            DimensionsVM dimensionsVM = Mapper.MapDimensionsVM(setDimensions.Content);

            return dimensionsVM;
        }

        public EditItemVM UpdateExponentItem(int id, EditItemVM editItem)
        {
            var itemExist = ItemRepository.GetSingleOrDefault(id);

            if (!itemExist.IsSuccess)
            {
                return null;
            }

            itemExist.Content.Name = editItem.Name;
            itemExist.Content.Description = editItem.ImageDescription;
            itemExist.Content.Creator = editItem.Creator;

            if (editItem.Image != null)
            {
                const string folder = "ExponentItems";

                itemExist.Content.Image = EditImage(folder, editItem.Image, itemExist.Content.Image);

            }

            var response = ItemRepository.Update(itemExist.Content);

            if (!response.IsSuccess)
            {
                return null;
            }

            var newEditItem = new EditItemVM()
            {
                Name = response.Content.Name,
                Creator = response.Content.Creator,
                ImageDescription = response.Content.Description,
                ImagePath = response.Content.Image
            };

            return newEditItem;;
        }

        public bool RemoveItem(int id)
        {
            var itemExist = ItemRepository.GetSingleOrDefault(id);

            if (!itemExist.IsSuccess)
            {
                return false;
            }

            var itemDimensions = DimensionsRepository.Find(dimensions => dimensions.ExponentItemId == id).ToList();

            var dimensionsRespone = DimensionsRepository.RemoveRange(itemDimensions);

            const string folder = "ExponentItems";

            DeleteImage(folder, itemExist.Content.Image);

            var response = ItemRepository.Remove(itemExist.Content);

            if (!response.IsSuccess)
            {
                return false;
            }

            return true;
        }

        public bool RemoveDimensions(int id)
        {
            var dimensionsExist = DimensionsRepository.GetSingleOrDefault(id);

            if (!dimensionsExist.IsSuccess)
            {
                return false;
            }

            var response = DimensionsRepository.Remove(dimensionsExist.Content);

            if (!response.IsSuccess)
            {
                return false;
            }

            return true;
        }

        public bool RemoveItems(int exhbitionId)
        {
            var itemsToRemove = GetExhibitionItems(exhbitionId);

            const string folder = "ExponentItems";

            foreach (var item in itemsToRemove)
            {
                if(!DeleteImage(folder, item.Image))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> AddColection(CollectionVM collectionItem)
        {
            var userExist = await UserManager.FindByNameAsync(collectionItem.Username);

            if (userExist == null)
            {
                return false;
            }

            List<CollectionItem> collection = new List<CollectionItem>();

            foreach (var item in collectionItem.Collection)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    collection.Add(new CollectionItem()
                    {
                        Name = item.Name,
                        Image = item.Image,
                        Description = item.Description,
                        Creator = item.Creator,
                        Dimensions = item.Dimensions,
                        Price = item.Price,
                        ExhibitionTitle = item.Exhibition,
                        Organizer = item.Organizer,
                        UserId = userExist.Id,
                        ExhibitionId = item.ExhibitionId
                    });
                }
            }

            var response = CollectionRepository.AddRange(collection);

            if(!response.IsSuccess)
            {
                return false;
            }

            return true;
        }

        public async  Task<List<CollectionItemVM>> GetCollection(string username)
        {
            var userExist = await UserManager.FindByNameAsync(username);

            if (userExist == null)
            {
                throw new Exception("User doesnpt exist!");
            }

            var response = CollectionRepository.Find(item => item.UserId == userExist.Id).Select(item => new CollectionItemVM()
            {
                Name = item.Name,
                Image = item.Image,
                Description = item.Description,
                Creator = item.Creator,
                Dimensions = item.Dimensions,
                Price = item.Price,
                Exhibition = item.ExhibitionTitle,
                Organizer = item.Organizer
            }).ToList();

            return response;
        }

        public int GetSoledItemCount(int exhibitionId)
        {
            int soledItemsCount = CollectionRepository.Find(collectionItem => collectionItem.ExhibitionId == exhibitionId).Count;

            return soledItemsCount;
        }

        // Methods for generating test data
        public string TestItemUpload(IFormFile file)
        {
            const string folder = "ProfilePictures";

           return EditImage(folder, file, null);
        }

        public void ExponentsUpload(int id, TestItemUploadVM testItemUpload, List<DimensionsVM> dimensions)
        {
            var repoResponse = ItemRepository.Add(new ExponentItem()
            {
                Name = testItemUpload.Name,
                Description = testItemUpload.Description,
                Creator = testItemUpload.Creator,
                ExhibitionId = id,
                Image = testItemUpload.Image
            });

            foreach (var dimension in dimensions)
            {
                AddDimensions(repoResponse.Content.Id, dimension);
            }
        }

        public double GetExhibitionProfit(int exhibitionId)
        {
            double profit = CollectionRepository.Find(collectionItem => collectionItem.ExhibitionId == exhibitionId).Sum(collectionItem => collectionItem.Price);

            return profit;
        }
    }
}
