using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Producer.Commands
{
    public class ManageProducerCommand
    {
        [Required]
        public int Id { get; set; }
    }
}