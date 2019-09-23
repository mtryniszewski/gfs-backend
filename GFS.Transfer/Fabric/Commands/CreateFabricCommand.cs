using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Fabric.Commands
{
    public class CreateFabricCommand
    {
        [Required]
        public float Thickness { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ProducerCode { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public int ProducerId { get; set; }
    }
}