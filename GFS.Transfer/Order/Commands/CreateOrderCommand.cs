using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Order.Commands
{
    public class CreateOrderCommand
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}