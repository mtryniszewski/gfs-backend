using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Fabric.Commands
{
    public class ManageFabricCommand
    {
        [Required]
        public int Id { get; set; }
    }
}