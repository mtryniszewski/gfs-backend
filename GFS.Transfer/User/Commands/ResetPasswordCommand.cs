using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.User.Commands
{
    public class ResetPasswordCommand
    {
        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
}