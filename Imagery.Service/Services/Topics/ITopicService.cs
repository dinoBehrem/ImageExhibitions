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
        List<TopicVM> SetExhibitionTopic(int exhbitionId, List<TopicVM> topics);
        List<TopicVM> GetAllTopics();
        List<TopicVM> GetExhibitionTopics(int exhibitionId);
        string RemoveExhibitionTopic(Exhibition exhbition, int topicId);
        TopicVM AssignTopic(int exhbitionId, int topicId);

        // Test methods
        void TestTopics(int exhibitionId);
    }
}
