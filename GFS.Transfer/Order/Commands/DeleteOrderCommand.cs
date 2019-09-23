using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Order.Commands
{
    public class DeleteOrderCommand
    {
        [Required]
        public int Id { get; set; }
    }
}