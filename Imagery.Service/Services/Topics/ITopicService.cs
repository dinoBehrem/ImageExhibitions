using Imagery.Core.Models;
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
        void Create(string name);
        TopicVM SetExhibitionTopic(int exhbitionId, int topicId);
        List<TopicVM> GetAllTopics();
        List<TopicVM> GetExhibitionTopics(int exhibitionId);
        string RemoveExhibitionTopic(Exhibition exhbition, int topicId);

        void TestTopics(int exhibitionId);
    }
}
