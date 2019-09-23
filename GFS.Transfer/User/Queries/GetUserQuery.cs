using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.User.Queries
{
    public class GetUserQuery
    {
        [Required]
        public string Id { get; set; }
    }
}