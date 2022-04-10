using RVezy.Domain.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVezy.Infra.Infra.Repositories
{
    public class BaseRepository
    {
        protected async Task<IEnumerable<TEntity>> Pagination<TEntity>(IQueryable<TEntity> query, PageOptions pageOptions)
        {
            if (pageOptions is null)
            {
                return await Task.FromResult(query.AsEnumerable());
            }
            pageOptions.Page = pageOptions.Page < 1 ? 1 : pageOptions.Page;
            var skip = (pageOptions.Page - 1) * pageOptions.Count;
            skip = skip < 0 ? 0 : skip;
            return await Task.FromResult(query.Skip(skip).Take(pageOptions.Count).AsEnumerable());
        }
    }
}
