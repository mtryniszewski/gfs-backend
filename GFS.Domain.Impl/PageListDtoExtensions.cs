using System.Linq;
using System.Threading.Tasks;
using GFS.Transfer.Shared;
using Microsoft.EntityFrameworkCore;

namespace GFS.Domain.Impl
{
    public static class PageListDtoExtensions
    {
        public static PageListDto<T> ToPagedList<T>(this IQueryable<T> queryable, ListQuery query)
        {
            var count = queryable.Count();

            var results = queryable
                .Skip(query.PageSize * query.Page)
                .Take(query.PageSize)
                .ToList();

            return new PageListDto<T>
            {
                Page = query.Page,
                PageSize = query.PageSize,
                Count = results.Count,
                Total = count,
                Items = results
            };
        }

        public static async Task<PageListDto<T>> ToPagedListAsync<T>(this IQueryable<T> queryable, ListQuery query)
        {
            var count = await queryable.CountAsync();

            var results = await queryable
                .Skip(query.PageSize * query.Page)
                .Take(query.PageSize)
                .ToListAsync();

            return new PageListDto<T>
            {
                Page = query.Page,
                PageSize = query.PageSize,
                Count = results.Count,
                Total = count,
                Items = results
            };
        }
    }
}