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

        public List<TopicVM> SetExhibitionTopic(int exhbitionId, List<TopicVM> topics)
        {
            // Get all exhibition topics
            var exhibitionTopics = TopicsExhibitionRepository.Find(et => et.ExhibitionId == exhbitionId);

            // Remove old exhibition topics
            var remove = TopicsExhibitionRepository.RemoveRange(exhibitionTopics);

            // Assign new topics to exhibition
            var assign = TopicsExhibitionRepository.AddRange(topics.Select(topic => new ExhibitionTopics() { ExhibitionId = exhbitionId, TopicId = topic.Id}).ToList());

            if (!assign.IsSuccess)
            {
                return null;
            }

            // Get new exhibition topics
            var assignedTopics = GetExhibitionTopics(exhbitionId);

            return assignedTopics;
         }

        public List<TopicVM> GetExhibitionTopics(int exhibitionId)
        {
            var topics = TopicsExhibitionRepository.Find(top => top.ExhibitionId == exhibitionId).Select(topic => new TopicVM() { Id = topic.TopicId, Name = GetTopic(topic.TopicId).Name, isAssigned = true }).ToList();

            return topics;
        }

        public string RemoveExhibitionTopic(Exhibition exhbition, int topicId)
        {
            var topicExhibirionExist = TopicsExhibitionRepository.Find(et => et.ExhibitionId == exhbition.Id && et.TopicId == topicId).FirstOrDefault();

            if (topicExhibirionExist == null)
            {
                return "Exhbition doesn't have this topic!";
            }

            var response = TopicsExhibitionRepository.Remove(topicExhibirionExist);

            if (!response.IsSuccess)
            {
                return "Error, not removed";
            }

            return response.Message;
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

        public TopicVM AssignTopic(int exhbitionId, int topicId)
        {
            var topicExist = TopicsRepository.GetSingleOrDefault(topicId);

            var topicExhibirionExist = TopicsExhibitionRepository.Find(et => et.ExhibitionId == exhbitionId && et.TopicId == topicId).FirstOrDefault();


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


        // Methods for generating test data

        public void TestTopics(int exhibitionId)
        {
            List<TopicVM> topics = GetAllTopics();

            Random random = new Random();

            int topicCount = random.Next(1, 3);

            List<int> exhibitionTopics = new List<int>();
           
            for (int i = 0; i < topicCount; i++)
            {
                int index = random.Next(0, topics.Count);

                if (!exhibitionTopics.Contains(topics[index].Id))
                {
                    exhibitionTopics.Add(topics[index].Id);
                    AssignTopic(exhibitionId, topics[index].Id);
                }
            }
        }

        public void Create(string name)
        {
            var response = TopicsRepository.Add(new Topic() { Name = name });
        }
    }
}
