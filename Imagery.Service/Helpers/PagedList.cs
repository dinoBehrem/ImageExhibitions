using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.Helpers
{
    public class PagedList<TEntity> : List<TEntity>
    {
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int CurentPage { get; private set; }

        public bool HasNextPage => CurentPage < TotalPages;
        public bool HasPreviousPage => CurentPage > 1;

        public PagedList(List<TEntity> entities, int totalCount, PageParameters parameters)
        {
            TotalCount = totalCount;
            PageSize = parameters.PageSize;
            CurentPage = parameters.PageSize;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)parameters.PageSize);

            AddRange(entities);
        }

        public static PagedList<TEntity> ToPagedList(IQueryable<TEntity> source, PageParameters parameters)
        {
            int count = source.Count();

            List<TEntity> items = source.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize).ToList();

            PagedList<TEntity> entities = new PagedList<TEntity>(items, count, parameters);

            return entities;
        }
    }
}
