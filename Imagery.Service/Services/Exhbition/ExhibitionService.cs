using Imagery.Core.Models;
using Imagery.Repository.Repository;
using Imagery.Service.ViewModels.Exhbition;
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

        public ExhibitionService(UserManager<User> userManager, IRepository<Exhibition> exhbitionRepository, IRepository<User> userRepository)
        {
            UserManager = userManager;
            ExhibitionRepository = exhbitionRepository;
            UserRepository = userRepository;
        }
        public async Task<Response> Create(ExhbitionCreationVM exhibitionCreation)
        {
            // check if organizer exists
            var user = await UserManager.FindByNameAsync(exhibitionCreation.Organizer);

            if (user == null)
            {
                return new Response()
                {
                    Status = "Error",
                    Message = "You are not signed user, please sign in!",
                    IsSuccess = false,
                };
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
                return new Response() 
                {
                    Status = "Error",
                    Message = result.Message,
                    IsSuccess = false,
                    Errors = new List<string>() { result?.InnerMessage }
                };
            }

            return new Response()
            {
                Status = "Success",
                Message = "Exhbition successfully organized!",
                IsSuccess = true,
            };
        }

        public List<ExhibitionVM> Exhibitions()
        {
            var exhibitions = ExhibitionRepository.GetAll().Select(exhibition => new ExhibitionVM()
            {
                Title = exhibition.Title,
                Description = exhibition.Description,
                Organizer = toUserVM(exhibition.OrganizerId),
                Date = exhibition.Date
            }).ToList();

            return exhibitions;
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
    }
}
