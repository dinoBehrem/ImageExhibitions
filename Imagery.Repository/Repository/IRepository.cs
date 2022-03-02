using Imagery.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
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
        void SaveChanges();
    }
}
