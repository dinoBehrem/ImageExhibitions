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

        public RepositoryResponse<TEntity> Add(TEntity entity)
        {
            RepositoryResponse<TEntity> response = new RepositoryResponse<TEntity>();

            try
            {
                Entities.Add(entity);

                ImageryContext.SaveChanges();
               
                response.Status = "Success";
                response.Message = "Entity successfully added!";
                response.IsSuccess = true;
                response.Content = entity;
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = ex.Message;
                response.InnerMessage = ex.InnerException?.Message;
                response.IsSuccess = false;
                response.Content = null;
            }

            return response;
        }

        public RepositoryResponse<TEntity> GetSingleOrDefault(int id)
        {
            TEntity entity = Entities.Find(id);

            RepositoryResponse<TEntity> response = new RepositoryResponse<TEntity>();

            if (entity == null)
            {
                response.Status = "Error";
                response.Message = "Entity not found!";
                response.IsSuccess = false;
                response.Content = null;
            }
            else
            {
                response.Status = "Success";
                response.Message = "Entity successfully found!";
                response.IsSuccess = true;
                response.Content = entity;
            }

            return response;
        }

        public void SaveChanges()
        {
            ImageryContext.SaveChanges();
        }

        public RepositoryResponse<TEntity> Update(TEntity entity)
        {
            RepositoryResponse<TEntity> response = new RepositoryResponse<TEntity>();

            try
            {
                ImageryContext.Entry(entity).State = EntityState.Modified;
                SaveChanges();

                response.Status = "Success";
                response.Message = "Entity successfully added!";
                response.IsSuccess = true;
                response.Content = entity;
            }
            catch (Exception ex)
            {
                response.Status = "Error";
                response.Message = ex.Message;
                response.InnerMessage = ex.InnerException?.Message;
                response.IsSuccess = false;
                response.Content = null;
            }

            return response;
        }
    }
}
