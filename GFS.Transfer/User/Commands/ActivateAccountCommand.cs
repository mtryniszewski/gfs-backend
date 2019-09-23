using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.User.Commands
{
    public class ActivateAccountCommand
    {
    
        [Required]
        public string AccountActivationToken { get; set; }
    }
}