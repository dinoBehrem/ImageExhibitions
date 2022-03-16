using Imagery.Core.Models;
using Imagery.Repository.Repository;
using Imagery.Service.Helpers;
using Imagery.Service.Services.Image;
using Imagery.Service.Services.Topics;
using Imagery.Service.ViewModels.Exhbition;
using Imagery.Service.ViewModels.Image;
using Imagery.Service.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imagery.Service.Services.Exhbition
{
    public class ExhibitionService : IExhibitionService
    {
        private readonly UserManager<User> UserManager;
        private readonly IRepository<Exhibition> ExhibitionRepository;
        private readonly IRepository<User> UserRepository;
        private readonly IImageService ImageService;
        private readonly ITopicService TopicService;

        public ExhibitionService(UserManager<User> userManager, IRepository<Exhibition> exhbitionRepository, IRepository<User> userRepository, IImageService imageService, ITopicService topicService)
        {
            UserManager = userManager;
            ExhibitionRepository = exhbitionRepository;
            UserRepository = userRepository;
            ImageService = imageService;
            TopicService = topicService;
        }

        public async Task<ExhibitionVM> Create(ExhbitionCreationVM exhibitionCreation)
        {
            // check if organizer exists
            var user = await UserManager.FindByNameAsync(exhibitionCreation.Organizer);

            if (user == null)
            {
                return null;
            }

            // create exhibition

            var result = ExhibitionRepository.Add(new Exhibition()
            {
                Title = exhibitionCreation.Title,
                Description = exhibitionCreation?.Description,
                Date = exhibitionCreation.StartingDate,
                ExpiringTime = exhibitionCreation.StartingDate.AddHours(2),
                Organizer = user,
                OrganizerId = exhibitionCreation.Organizer
            });

            if (!result.IsSuccess)
            {
                return null;
            }

            return new ExhibitionVM()
            {
                Id = result.Content.Id,
                Title = result.Content.Title,
                Description = result.Content.Description,
                Date = result.Content.Date,
                Organizer = Mapper.MapUserVM(user),
                Items = null,
                Cover = result.Content.CoverImage,
            };
        }

        public List<ExhibitionVM> Exhibitions()
        {
            var exhibitions = ExhibitionRepository.GetAll().Select(exhibition => new ExhibitionVM()
            {
                Id = exhibition.Id,
                Title = exhibition.Title,
                Description = exhibition.Description,
                Organizer = toUserVM(exhibition.OrganizerId),
                Date = exhibition.Date,
                Cover = exhibition.CoverImage,
                Items = ExhbitionItems(exhibition.Id),
                Topics = GetExhibitionTopics(exhibition.Id),
                Expired = exhibition.ExpiringTime < DateTime.Now
            }).ToList();

            return exhibitions;
        }

        public ExhibitionVM GetById(int id)
        {
            var repoResponse = ExhibitionRepository.GetSingleOrDefault(id);

            if (!repoResponse.IsSuccess)
            {
                return null;
            }

            ExhibitionVM exhibition = new ExhibitionVM()
            {
                Id = id,
                Title = repoResponse.Content.Title,
                Date = repoResponse.Content.Date,
                Description = repoResponse.Content.Description,
                Organizer = toUserVM(repoResponse.Content.OrganizerId),
                Cover = repoResponse.Content.CoverImage,
                Items = ExhbitionItems(id),
                Topics = GetExhibitionTopics(id)
            };

            return exhibition;
        }

        public string SetExhibitionCover(CoverImageVM cover)
        {
            if (string.IsNullOrEmpty(cover.CoverImage))
            {
                return null;
            }

            var response = ExhibitionRepository.GetSingleOrDefault(cover.ExhibitionId);

            if (!response.IsSuccess)
            {
                return null;
            }

            response.Content.CoverImage = cover.CoverImage;
            ExhibitionRepository.SaveChanges();

            return cover.CoverImage;
        }

        public EditExhibitionVM UpdateExhibition(EditExhibitionVM input)
        {
            var exhibition = ExhibitionRepository.GetSingleOrDefault(input.Id);

            if (!exhibition.IsSuccess)
            {
                return null;
            }

            exhibition.Content.Title = input.Title;
            exhibition.Content.Date = input.Date;
            exhibition.Content.Description = input.Description;

            var result = ExhibitionRepository.Update(exhibition.Content);

            if (!result.IsSuccess)
            {
                return null;
            }

            EditExhibitionVM editExhibition = new EditExhibitionVM() { Id = result.Content.Id, Title = result.Content.Title, Description = result.Content.Description, Date = result.Content.Date};
            //response.Items = ExhbitionItems(response.Id);
            //response.Organizer = toUserVM(exhibition.Content.OrganizerId);

            return editExhibition;
        }

        private UserVM toUserVM(string id)
        {
            User user = UserRepository.GetAll().Where(user => user.Id == id).Single();

            return new UserVM()
            {
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                Picture = user.ProfilePicture
            };
        }

        private List<ExponentItemVM> ExhbitionItems(int id)
        {
            return ImageService.GetExhibitionItems(id).ToList();
        }

        public TopicVM AssignTopic(AssignTopicVM assignTopic)
        {
            var exhibitionExist = ExhibitionRepository.GetSingleOrDefault(assignTopic.ExhibitionId);

            if (!exhibitionExist.IsSuccess)
            {
                return null;
            }

            var assignedTopic = TopicService.SetExhibitionTopic(assignTopic.ExhibitionId, assignTopic.TopicId);

            return assignedTopic;
        }

        private List<TopicVM> GetExhibitionTopics(int id)
        {
            var topics = TopicService.GetExhibitionTopics(id);

            return topics;
        }

        public List<ExhibitionVM> UserExhibitions(string username)
        {
            List<ExhibitionVM> exhibitions = Exhibitions().Where(exhibition => exhibition.Organizer.Username == username).ToList();

            return exhibitions;
        }

        public bool RemoveExhbition(int exhbitionId)
        {
            var exhbitionExist = ExhibitionRepository.GetSingleOrDefault(exhbitionId);

            if (!exhbitionExist.IsSuccess)
            {
                return false;
            }

            ImageService.RemoveItems(exhbitionId);
            var result = ExhibitionRepository.Remove(exhbitionExist.Content);

            if (!result.IsSuccess)
            {
                return false;
            }

            return true;
        }
    }
}
