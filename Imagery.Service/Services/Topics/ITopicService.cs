using Imagery.Service.ViewModels.Exhbition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.Services.Topics
{
    public interface ITopicService
    {
        TopicVM SetExhibitionTopic(int exhbitionId, int topicId);
        List<TopicVM> GetAllTopics();
        List<TopicVM> GetExhibitionTopics(int exhibitionId);
    }
}
