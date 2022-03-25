using Imagery.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Repository.Repository
{
    public interface IRepository<TEntity>
    {
        List<TEntity> GetAll();
        RepositoryResponse<TEntity> Add(TEntity entity);
        RepositoryResponse<TEntity> GetSingleOrDefault(int id);
        RepositoryResponse<TEntity> Update(TEntity entity);
        RepositoryResponse<TEntity> Remove(TEntity entity);
        RepositoryResponse<TEntity> AddRange(List<TEntity> entity);
        RepositoryResponse<TEntity> RemoveRange(List<TEntity> entity);
        List<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        void SaveChanges();
        int TotalEntitiesCount(Expression<Func<TEntity, bool>> expression);
    }
}
