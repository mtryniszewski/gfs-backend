using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Order.Commands
{
    public class ManageOrderCommand
    {
        [Required]
        public int Id { get; set; }
    }
}