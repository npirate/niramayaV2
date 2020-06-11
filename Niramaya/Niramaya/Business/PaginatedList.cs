using Niramaya.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Niramaya.Business
{
    public class PaginatedList
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }


        public PaginatedList(int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);            
        }

        public void UpdatePager(int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        //public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        //{
        //    var count = await source.CountAsync();
        //    var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        //    return new PaginatedListGeneric<T>(items, count, pageIndex, pageSize);
        //}
    }
}
