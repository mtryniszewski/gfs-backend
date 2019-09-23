using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Producer.Commands
{
    public class DeleteProducerCommand
    {
        [Required]
        public int Id { get; set; }
    }
}