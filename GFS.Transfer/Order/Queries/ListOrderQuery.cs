using System.ComponentModel.DataAnnotations;
using GFS.Transfer.Shared;

namespace GFS.Transfer.Order.Queries
{
    public class ListOrderQuery : ListQuery
    {
        [Required]
        public string UserId { get; set; }
    }
}