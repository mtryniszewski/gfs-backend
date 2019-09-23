using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.User.Commands
{
    public class ManageUserCommand
    {
        [Required]
        public string Id { get; set; }
    }
}