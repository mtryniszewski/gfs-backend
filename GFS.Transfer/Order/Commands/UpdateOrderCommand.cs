using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Order.Commands
{
    public class UpdateOrderCommand
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }
    }
}