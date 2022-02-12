using Imagery.Repository.Context;
using Imagery.Repository.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Repository.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ImageryContext ImageryContext;
        private readonly DbSet<TEntity> Entities;
        public Repository(ImageryContext imageryContext)
        {
            ImageryContext = imageryContext;
            Entities = ImageryContext.Set<TEntity>();
        }
        public List<TEntity> GetAll()
        {
            return Entities.ToList();
        }

        public RepositoryResponse Add(TEntity entity)
        {
            try
            {
                Entities.Add(entity);

                ImageryContext.SaveChanges();
                return new RepositoryResponse()
                {
                    Status = "Success",
                    Message = "Entity successfully added!",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {

                return new RepositoryResponse() 
                {
                    Status = "Error",
                    Message = ex.Message,
                    InnerMessage = ex.InnerException?.Message,
                    IsSuccess = false
                };
            }
            
        }
    }
}
