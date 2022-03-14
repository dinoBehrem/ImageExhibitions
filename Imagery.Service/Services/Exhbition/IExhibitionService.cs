﻿using Imagery.Service.ViewModels.Exhbition;
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
        EditExhibitionVM UpdateExhibition(EditExhibitionVM exhibition);
        //ExhibitionVM UpdateExhibition(ExhibitionVM exhibition);
        string SetExhibitionCover(CoverImageVM cover);
        List<ExhibitionVM> UserExhibitions(string username);
        TopicVM AssignTopic(AssignTopicVM assignTopic);
    }
}
