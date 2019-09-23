using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GFS.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace GFS.Data.Model.Entities
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public Permissions Permissions { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchival { get; set; }
        public string PasswordResetToken { get; set; }
        public string AccountActivationToken { get; set; }
        public List<Order> Orders { get; set; }
    }
}