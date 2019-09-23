using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.User.Commands
{
    public class ChangePasswordCommand
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string OldPassword { get; set; }
    }
}