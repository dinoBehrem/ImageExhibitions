using Imagery.Core.Models;
using Imagery.Repository.Repository;
using Imagery.Service.Helpers;
using Imagery.Service.ViewModels.Exhbition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.Services.Topics
{
    public class TopicService : ITopicService
    {
        private readonly IRepository<Topic> TopicsRepository;
        private readonly IRepository<ExhibitionTopics> TopicsExhibitionRepository;

        public TopicService(IRepository<Topic> topicsRepository, IRepository<ExhibitionTopics> topicsExhibitionRepository)
        {
            TopicsRepository = topicsRepository;
            TopicsExhibitionRepository = topicsExhibitionRepository;
        }

        public List<TopicVM> GetAllTopics()
        {
            return TopicsRepository.GetAll().Select(topic => new TopicVM() { Id = topic.Id, Name = topic.Name, isAssigned = false }).ToList();
        }

        public TopicVM SetExhibitionTopic(int exhbitionId, int topicId)
        {
            var topicExist = TopicsRepository.GetSingleOrDefault(topicId);

            if (!topicExist.IsSuccess)
            {
                return null;
            }

            var assign = TopicsExhibitionRepository.Add(new ExhibitionTopics() { ExhibitionId = exhbitionId, TopicId = topicId });

            if (!assign.IsSuccess)
            {
                return null;
            }

            TopicVM topicVM = Mapper.MapTopicVM(topicExist.Content);

            return topicVM;
         }

        public List<TopicVM> GetExhibitionTopics(int exhibitionId)
        {
            var topics = TopicsExhibitionRepository.GetAll().Where(top => top.ExhibitionId == exhibitionId).Select(topic => new TopicVM() { Id = topic.TopicId, Name = GetTopic(topic.TopicId).Name, isAssigned = true }).ToList();

            return topics;
        }

        private Topic GetTopic(int topicId)
        {
            var topic = TopicsRepository.GetSingleOrDefault(topicId);

            if (!topic.IsSuccess)
            {
                return null;
            }

            return topic.Content;
        }
    }
}
