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
        private int TotalCount = 0;
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
                throw new Exception("User doesn't exist, try again!");
            }

            // create exhibition
            var result = ExhibitionRepository.Add(new Exhibition()
            {
                Title = exhibitionCreation.Title,
                Description = CheckIfNullOrEmpty(exhibitionCreation.Description),
                Date = exhibitionCreation.StartingDate,
                ExpiringTime = exhibitionCreation.StartingDate.AddHours(2),
                Organizer = user,
                OrganizerId = exhibitionCreation.Organizer
            });


            // check if addtion was successfull
            if (!result.IsSuccess)
            {
                throw new Exception(result.Message);

            }

            // if successfull convert to view model
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
            // get all exhibitions that haven't expired
            List<ExhibitionVM> exhibitions = ExhibitionRepository.Find(exhibition => exhibition.ExpiringTime > DateTime.Now).Select(exhibition => new ExhibitionVM()
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

            exhibitions.Sort((exhibition, compareExhbition) => exhibition.Date.CompareTo(compareExhbition.Date));

            TotalCount = exhibitions.Count;

            return exhibitions;
        }

        public ExhibitionVM GetById(int id)
        {
            // get requested exhibition
            var repoResponse = ExhibitionRepository.GetSingleOrDefault(id);

            // check for success
            if (!repoResponse.IsSuccess)
            {
                throw new Exception(repoResponse.Message);
            }

            // conver to exhibition view model
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
            // check if image is valid
            if (string.IsNullOrEmpty(cover.CoverImage))
            {
                throw new Exception("Please select image to set as cover!");
            }

            // get requested exhibition
            var response = ExhibitionRepository.GetSingleOrDefault(cover.ExhibitionId);


            // check for success
            if (!response.IsSuccess)
            {
                throw new Exception(response.Message);
            }

            // set cover image
            response.Content.CoverImage = cover.CoverImage;

            ExhibitionRepository.SaveChanges();

            // return cover image server path
            return cover.CoverImage;
        }

        public EditExhibitionVM UpdateExhibition(int exhibitionId, EditExhibitionVM input)
        {
            var exhibition = ExhibitionRepository.GetSingleOrDefault(exhibitionId);

            if (!exhibition.IsSuccess)
            {
                throw new Exception("Exhibition doesn't exist!");
            }

            exhibition.Content.Title = input.Title;
            exhibition.Content.Date = input.Date;
            exhibition.Content.ExpiringTime = input.Date.AddHours(2);
            exhibition.Content.Description = input.Description;

            var result = ExhibitionRepository.Update(exhibition.Content);

            var updateTopics = TopicService.SetExhibitionTopic(exhibitionId, input.Topics);

            if (!result.IsSuccess)
            {
                throw new Exception(result.Message);
            }

            EditExhibitionVM editExhibition = new EditExhibitionVM() 
            { 
                Title = result.Content.Title,
                Description = result.Content.Description,
                Date = result.Content.Date,
                Topics = updateTopics
            };

            return editExhibition;
        }

        public TopicVM AssignTopic(AssignTopicVM assignTopic)
        {
            var exhibitionExist = ExhibitionRepository.GetSingleOrDefault(assignTopic.ExhibitionId);

            if (!exhibitionExist.IsSuccess)
            {
                return null;
            }

            var assignedTopic = TopicService.AssignTopic(assignTopic.ExhibitionId, assignTopic.TopicId);

            return assignedTopic;
        }

        public List<ExhibitionVM> UserExhibitions(string username)
        {
            List<ExhibitionVM> exhibitions = Exhibitions().Where(exhibition => exhibition.Organizer.Username == username).ToList();

            return exhibitions;
        }

        public async Task<List<MyExhibitionVM>> MyExhibitions(string username)
        {
            var userExist = await UserManager.FindByNameAsync(username);

            if (userExist == null)
            {
                throw new Exception("User doesn't exist");
            }

            List<MyExhibitionVM> exhibitions = ExhibitionRepository.Find(exhibition => exhibition.OrganizerId == userExist.Id).Select(exhibition => new MyExhibitionVM()
            {
                Id = exhibition.Id,
                Title = exhibition.Title,
                Description = exhibition.Description,
                Date = exhibition.Date,
                Cover = exhibition.CoverImage,
                Items = ExhbitionItems(exhibition.Id).Count,
                Topics = GetExhibitionTopics(exhibition.Id),
                Started = exhibition.Date < DateTime.Now,
                Expired = exhibition.ExpiringTime < DateTime.Now,
                Subscribers = GetExibitionsSubscribers(exhibition.Id),
                SoldItems = ImageService.GetSoledItemCount(exhibition.Id),
                Profit = ImageService.GetExhibitionProfit(exhibition.Id)
            }).ToList();
            
            TotalCount = exhibitions.Count;

            exhibitions = PagedList<MyExhibitionVM>.ToPagedList(exhibitions.AsQueryable(), new PageParameters());

            return exhibitions;
        }

        public bool RemoveExhbition(int exhbitionId)
        {
            // get requested exhibition
            var exhbitionExist = ExhibitionRepository.GetSingleOrDefault(exhbitionId);

            // check if exhibition exist
            if (!exhbitionExist.IsSuccess)
            {
                throw new Exception(exhbitionExist.Message);
            }

            // check if exhibition expired
            if (exhbitionExist.Content.ExpiringTime > DateTime.Now)
            {
                throw new Exception("Can't remove expired exhibitions!");
            }

            // remove exhibition items
            ImageService.RemoveItems(exhbitionId);

            // remove exhibition
            var result = ExhibitionRepository.Remove(exhbitionExist.Content);

            // check for success
            if (!result.IsSuccess)
            {
                throw new Exception(result.Message);
            }

            return true;
        }

        public async Task<bool> Subscribe(ExhibitionSubscriptionVM exhibitionSubscription)
        {
            var userExist = await UserManager.FindByNameAsync(exhibitionSubscription.Username);

            var exhibitionExist = ExhibitionRepository.GetSingleOrDefault(exhibitionSubscription.ExhibitionId);

            if (exhibitionExist.Content == null || userExist == null)
            {
                throw new Exception("Invalid exhibition, try again!");
            }

            var response = ExhibitionSubsRepository.Add(new ExhibitionSubscription() { ExhibitionId = exhibitionExist.Content.Id, UserId = userExist.Id });

            if (!response.IsSuccess)
            {
                throw new Exception("You've already subscribed to this exhibition!");
            }

            return true;
        }

        public async Task<bool> Unsubscribe(ExhibitionSubscriptionVM exhibitionSubscription)
        {
            var userExist = await UserManager.FindByNameAsync(exhibitionSubscription.Username);

            var exhibitionExist = ExhibitionRepository.GetSingleOrDefault(exhibitionSubscription.ExhibitionId);

            if (exhibitionExist.Content == null || userExist == null)
            {
                throw new Exception("Invalid exhibition, try again!");
            }

            var exhSub = ExhibitionSubsRepository.Find(sub => sub.ExhibitionId == exhibitionExist.Content.Id && sub.UserId == userExist.Id).FirstOrDefault();

            var response = ExhibitionSubsRepository.Remove(exhSub);

            if (!response.IsSuccess)
            {
                throw new Exception("You've already unsubscribed from this exhibition!");
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


        private string CheckIfNullOrEmpty(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return "";
            }

            return content;
        }

        // Testing pagination

        public List<ExhibitionVM> GetPagedExhbition(PageParameters parameters)
        {
            List<ExhibitionVM> exhibitions = PagedList<ExhibitionVM>.ToPagedList(Exhibitions().AsQueryable(), parameters);

            return exhibitions;
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


        // Filtering exhibitions
        public List<ExhibitionVM> GetFilteredExhbition(FilterVM filters, PageParameters parameters)
        {
            List<ExhibitionVM> exhibitions = Exhibitions().Where(exhibition =>
            (exhibition.AveragePrice >= filters?.AvgPriceMin || filters.AvgPriceMin == null) &&
            (exhibition.AveragePrice <= filters?.AvgPriceMax || filters.AvgPriceMax == null) &&
            (exhibition.Date >= filters.DateFrom || filters.DateFrom == null) &&
            (exhibition.Date <= filters.DateTo || filters.DateTo == null) &&
            (string.IsNullOrEmpty(filters.CreatorName) || exhibition.Organizer.Firstname.ToLower().Contains(filters.CreatorName.ToLower())) &&
            (string.IsNullOrEmpty(filters.Description) || exhibition.Description.ToLower().Contains(filters.Description.ToLower())) &&
            CheckForTopics(filters.Topics, exhibition.Topics)).ToList();

            List<ExhibitionVM> filterdExhitions = PagedList<ExhibitionVM>.ToPagedList(exhibitions.AsQueryable(), parameters);

            TotalCount = exhibitions.Count;

            return filterdExhitions;
        }

        public int GetTotalCount()
        {
            return TotalCount;
        }

        private bool CheckForTopics(List<string> topics, List<TopicVM> exhibitionTopics)
        {
            if (topics?.Count == 0 || topics == null)
            {
                return true;
            }

            foreach (var exhibitionTopic in exhibitionTopics)
            {
                if (!topics.Contains(exhibitionTopic.Name))
                {
                    return false;
                }
            }

            return true;
        }

        public List<ExhibitionVM> FilterByName(string title)
        {
            List<ExhibitionVM> source = Exhibitions().Where(exhibition => exhibition.Title.ToLower().Contains(title.ToLower())).ToList();
            List<ExhibitionVM> exhibitions = PagedList<ExhibitionVM>.ToPagedList(source.AsQueryable(), new PageParameters());

            TotalCount = source.Count;

            return exhibitions;
        }
    }
}
