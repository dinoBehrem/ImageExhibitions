using Imagery.Core.Models;
using Imagery.Repository.Repository;
using Imagery.Service.Helpers;
using Imagery.Service.Services.Image;
using Imagery.Service.ViewModels.Exhbition;
using Imagery.Service.ViewModels.Image;
using Imagery.Service.ViewModels.User;
using Microsoft.AspNetCore.Identity;
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

        public ExhibitionService(UserManager<User> userManager, IRepository<Exhibition> exhbitionRepository, IRepository<User> userRepository, IImageService imageService)
        {
            UserManager = userManager;
            ExhibitionRepository = exhbitionRepository;
            UserRepository = userRepository;
            ImageService = imageService;
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
                Title = exhibitionCreation.Title,
                Description = exhibitionCreation.Description,
                Date = exhibitionCreation.StartingDate,
                Organizer = Mapper.MapUserVM(user),
                Items = null
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
                Items = ExhbitionItems(exhibition.Id)
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
                Items = ExhbitionItems(id)
            };

            return exhibition;
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
            };
        }

        private List<ExponentItemVM> ExhbitionItems(int id)
        {
            return ImageService.GetExhibitionItems(id).ToList();
        }
    }
}
