using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Order.Queries
{
    public class GetOrderQuery
    {
        [Required]
        public int Id { get; set; }
    }
}