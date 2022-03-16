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
                Id = exponentItem.Id,
                Name = exponentItem.Name,
                Creator = exponentItem.Creator,
                Description = exponentItem.Description,
                Image = exponentItem.Image,
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
                Organizer = MapUserVM(exhibition.Organizer),
                Expired = exhibition.ExpiringTime < DateTime.Now,
            };

            return exhibitionVM;
        }

        public static DimensionsVM MapDimensionsVM(Dimensions dimensions)
        {
            if (dimensions == null)
            {
                return null;
            }

            DimensionsVM dimensionsVM = new DimensionsVM()
            {
                Dimension = dimensions.Dimension,
                Price = dimensions.Price,
                Id = dimensions.Id
            };

            return dimensionsVM;
        }

        public static TopicVM MapTopicVM(Topic topic)
        {
            if (topic == null)
            {
                return null;
            }

            TopicVM topicVM = new TopicVM()
            {
                Id = topic.Id,
                Name = topic.Name,
                isAssigned = true
            };

            return topicVM;
        }
    }
}
