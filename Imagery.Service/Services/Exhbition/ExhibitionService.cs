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
        private readonly IRepository<ExhibitionSubscription> ExhibitionSubsRepository;

        public ExhibitionService(UserManager<User> userManager, IRepository<Exhibition> exhbitionRepository, IRepository<User> userRepository, IImageService imageService, ITopicService topicService, IRepository<ExhibitionSubscription> exhibitionSubsRepository)
        {
            UserManager = userManager;
            ExhibitionRepository = exhbitionRepository;
            UserRepository = userRepository;
            ImageService = imageService;
            TopicService = topicService;
            ExhibitionSubsRepository = exhibitionSubsRepository;
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
                Started = result.Content.Date > DateTime.Now,
                Expired = result.Content.ExpiringTime < DateTime.Now
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
                Started = exhibition.Date < DateTime.Now,
                Expired = exhibition.ExpiringTime < DateTime.Now,
                Subscribers = GetExibitionsSubscribers(exhibition.Id)
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
                Topics = GetExhibitionTopics(id),
                Expired = repoResponse.Content.ExpiringTime >= DateTime.Now,
                Started = repoResponse.Content.Date >= DateTime.Now,
                Subscribers = GetExibitionsSubscribers(repoResponse.Content.Id)
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

        public async Task<bool> Subscribe(ExhibitionSubscriptionVM exhibitionSubscription)
        {
            var userExist = await UserManager.FindByNameAsync(exhibitionSubscription.Username);

            var exhibitionExist = ExhibitionRepository.GetSingleOrDefault(exhibitionSubscription.ExhibitionId);

            if (exhibitionExist.Content == null || userExist == null)
            {
                return false;
            }

            var response = ExhibitionSubsRepository.Add(new ExhibitionSubscription() { ExhibitionId = exhibitionExist.Content.Id, UserId = userExist.Id });

            if (!response.IsSuccess)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Unsubscribe(ExhibitionSubscriptionVM exhibitionSubscription)
        {
            var userExist = await UserManager.FindByNameAsync(exhibitionSubscription.Username);

            var exhibitionExist = ExhibitionRepository.GetSingleOrDefault(exhibitionSubscription.ExhibitionId);

            if (exhibitionExist.Content == null || userExist == null)
            {
                return false;
            }

            var exhSub = ExhibitionSubsRepository.Find(sub => sub.ExhibitionId == exhibitionExist.Content.Id && sub.UserId == userExist.Id).FirstOrDefault();

            if (exhSub == null)
            {
                return false;
            }

            var response = ExhibitionSubsRepository.Remove(exhSub);

            if (!response.IsSuccess)
            {
                return false;
            }

            return true;
        }
      
        private UserVM toUserVM(string id)
        {
            User user = UserRepository.Find(user => user.Id == id).Single();

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
       
        private List<TopicVM> GetExhibitionTopics(int id)
        {
            var topics = TopicService.GetExhibitionTopics(id);

            return topics;
        }

        private int GetExibitionsSubscribers(int id)
        {
            int count = ExhibitionSubsRepository.Find(exh => exh.ExhibitionId == id).Count;

            return count;
        }




        // Methods for generating test data
        public async Task<int> AddTestExhibitions(ExhbitionCreationVM exhbitionCreation, List<RegisterVM> registers)
        {
            // Create exhibition
            var response =  await Create(exhbitionCreation);

            // Assign topics to exhibition
            Random random = new Random();

            List<TopicVM> topics = TopicService.GetAllTopics();

            int topicCount = random.Next(1, 3);

            List<int> exhibitionTopics = new List<int>();

            for (int i = 0; i < topicCount; i++)
            {
                int index = random.Next(0, topics.Count);

                if (!exhibitionTopics.Contains(topics[index].Id))
                {
                    exhibitionTopics.Add(topics[index].Id);
                    AssignTopic(new AssignTopicVM() { ExhibitionId = response.Id, TopicId = topics[index].Id });
                }
            }

            // Subscribe to exhibition
            int subCount = random.Next(1, 5);

            List<ExhibitionSubscriptionVM> subscribes = new List<ExhibitionSubscriptionVM>();

            ExhibitionSubscriptionVM subscribeVM = new ExhibitionSubscriptionVM();

            for (int i = 0; i < subCount; i++)
            {
                subscribeVM.ExhibitionId = response.Id;
                int index = random.Next(0, registers.Count);

                subscribeVM.Username = registers[index].Username;

                if (!subscribes.Contains(subscribeVM))
                {
                    subscribes.Add(subscribeVM);
                    await Subscribe(subscribeVM);
                }
            }

            return response.Id;
        }

        public void TestItems(int id, TestItemUploadVM testItem, List<DimensionsVM> dimensions)
        {
            ImageService.ExponentsUpload(id, testItem, dimensions);
            SetExhibitionCover(new CoverImageVM() { ExhibitionId = id, CoverImage = testItem.Image });
        }
    }
}
