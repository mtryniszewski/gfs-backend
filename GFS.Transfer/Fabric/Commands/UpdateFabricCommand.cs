using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Fabric.Commands
{
    public class UpdateFabricCommand
    {
        [Required]
        public int Id { get; set; }

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