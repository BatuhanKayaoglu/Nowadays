using Microsoft.EntityFrameworkCore;

namespace Nowadays.API.Pagination
{
    public static class PagingExtensions
    {
        public static async Task<PagedViewModel<T>> GetPaged<T>(this IQueryable<T> query,
                                                                int currentPage,
                                                                int pageSize) where T : class

        {
            var count = await query.CountAsync(); 

            Page paging = new Page(currentPage, pageSize, count); 

            var data = await query.Skip(paging.Skip).Take(paging.PageSize).AsNoTracking().ToListAsync(); // We select how many data we skipped and bring the subsequent records. Like skip the first 20 records and bring the next 10.

            var result = new PagedViewModel<T>(data, paging);

            return result;
        }
    }
}
