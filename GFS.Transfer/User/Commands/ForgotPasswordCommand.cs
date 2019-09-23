using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.User.Commands
{
    public class ForgotPasswordCommand
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}