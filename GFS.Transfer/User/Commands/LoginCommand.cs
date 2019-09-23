using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.User.Commands
{
    public class LoginCommand
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}