using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.User.Commands
{
    public class CreateUserCommand
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}