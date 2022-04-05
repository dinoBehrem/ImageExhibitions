using Imagery.Service.ViewModels.Image;
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
        DimensionsVM AddDimensions(int id, DimensionsVM dimensions);
        bool RemoveDimensions(int id);
        EditItemVM UpdateExponentItem(int id, EditItemVM editItem);
        bool RemoveItem(int id);
        bool RemoveItems(int exhbitionId);
        Task<bool> AddColection(CollectionVM collectionItem);
        Task<List<CollectionItemVM>> GetCollection(string username);
        int GetSoledItemCount(int exhibitionId);
        double GetExhibitionProfit(int exhibitionId);

        // Methods for adding test data
        string TestItemUpload(IFormFile file);
        void ExponentsUpload(int id, TestItemUploadVM testItemUpload, List<DimensionsVM> dimensions);
    }
}
