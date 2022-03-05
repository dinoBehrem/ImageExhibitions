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
        List<ExhibitionVM> Exhibitions();
        ExhibitionVM GetById(int id);
        ExhibitionVM UpdateExhibition(ExhibitionVM exhibition);
        string SetExhibitionCover(CoverImageVM cover);

        TopicVM AssignTopic(AssignTopicVM assignTopic);
    }
}
