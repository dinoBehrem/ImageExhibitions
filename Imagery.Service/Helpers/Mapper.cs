using Imagery.Core.Models;
using Imagery.Service.ViewModels.Exhbition;
using Imagery.Service.ViewModels.Image;
using Imagery.Service.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.Helpers
{
    public static class Mapper
    {
        public static UserVM MapUserVM(User user)
        {
            if (user == null)
            {
                return null;
            }

            UserVM toUserVM = new UserVM()
            {
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                Picture = user.ProfilePicture,
            };

            return toUserVM;
        }

        public static ExponentItemVM MapExponentItemVM(ExponentItem exponentItem)
        {
            if (exponentItem == null)
            {
                return null;
            }

            ExponentItemVM exponent = new ExponentItemVM()
            {
                Name = exponentItem.Name,
                Creator = exponentItem.Creator,
                Description = exponentItem.Description,
                Dimensions = exponentItem.Dimensions,
                Image = exponentItem.Image,
                Price = exponentItem.Price
            };

            return exponent;
        }

        public static ExhibitionVM MapExhibitionVM(Exhibition exhibition)
        {
            if (exhibition == null)
            {
                return null;
            }

            ExhibitionVM exhibitionVM = new ExhibitionVM()
            {
                Id = exhibition.Id,
                Title = exhibition.Title,
                Description = exhibition.Description,
                Date = exhibition.Date,
                Cover = exhibition.CoverImage,
                Organizer = MapUserVM(exhibition.Organizer)
            };

            return exhibitionVM;
        }
    }
}
