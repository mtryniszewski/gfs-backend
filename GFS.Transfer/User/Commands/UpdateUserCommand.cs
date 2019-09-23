using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.User.Commands
{
    public class UpdateUserCommand
    {
        [Required]
        public string Id { get; set; }

       [Required]
        public string Surname { get; set; }

        [Required]
        public string Name { get; set; }
    }
}