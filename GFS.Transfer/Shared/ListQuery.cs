using System.Linq;

namespace GFS.Transfer.Shared
{
    public class ListQuery
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string[] SearchBy { get; set; }
    }

    public static class ListQueryExtensions
    {
        public static bool ShouldSearch(this ListQuery query)
        {
            return query.SearchBy != null && query.SearchBy.Any() && query.SearchBy.All(x => !string.IsNullOrEmpty(x));
        }
    }
}