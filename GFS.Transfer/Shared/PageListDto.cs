using System.Collections.Generic;

namespace GFS.Transfer.Shared
{
    public class PageListDto<T>
    {
        public List<T> Items { get; set; }

        public int Count { get; set; }

        public int Total { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}