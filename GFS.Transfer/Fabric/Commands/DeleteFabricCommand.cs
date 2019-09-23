using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Fabric.Commands
{
    public class DeleteFabricCommand
    {
        [Required]
        public int Id { get; set; }
    }
}