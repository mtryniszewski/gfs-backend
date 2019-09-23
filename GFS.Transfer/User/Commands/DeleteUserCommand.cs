using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.User.Commands
{
    public class DeleteUserCommand
    {
        [Required]
        public string Id { get; set; }
    }
}