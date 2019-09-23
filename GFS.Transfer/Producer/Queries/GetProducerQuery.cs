using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Producer.Queries
{
    public class GetProducerQuery
    {
        [Required]
        public int Id { get; set; }
    }
}