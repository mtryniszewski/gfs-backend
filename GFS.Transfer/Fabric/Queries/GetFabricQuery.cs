using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Fabric.Queries
{
    public class GetFabricQuery
    {
        [Required]
        public int Id { get; set; }
    }
}