using Imagery.Service.Helpers;
using Imagery.Service.ViewModels.Exhbition;
using Imagery.Service.ViewModels.Image;
using Imagery.Service.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.Services.Exhbition
{
    public interface IExhibitionService
    {
        Task<ExhibitionVM> Create(ExhbitionCreationVM exhibitionCreationVM);
        bool RemoveExhbition(int exhbitionId);
        List<ExhibitionVM> Exhibitions();
        ExhibitionVM GetById(int id);
        EditExhibitionVM UpdateExhibition(int exhibitionId, EditExhibitionVM exhibition);
        string SetExhibitionCover(CoverImageVM cover);

        List<ExhibitionVM> UserExhibitions(string username);
        Task<List<MyExhibitionVM>> MyExhibitions(string username);
        TopicVM AssignTopic(AssignTopicVM assignTopic);
        Task<bool> Subscribe(ExhibitionSubscriptionVM exhibitionSubscription);
        Task<bool> Unsubscribe(ExhibitionSubscriptionVM exhibitionSubscription);

        // Methods for adding test data
        Task<int> AddTestExhibitions(ExhbitionCreationVM exhbitionCreations, List<RegisterVM> registers);
        void TestItems(int id, TestItemUploadVM testItem, List<DimensionsVM> dimensions);


        // Pagination 
        List<ExhibitionVM> GetPagedExhbition(PageParameters parameters);
        
        // Filtering exhibitions
        List<ExhibitionVM> GetFilteredExhbition(FilterVM filters, PageParameters pageParameters);
        int GetTotalCount();
        List<ExhibitionVM> FilterByName(string title);
    }
}
