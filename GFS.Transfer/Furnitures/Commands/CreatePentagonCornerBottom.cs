using System.ComponentModel.DataAnnotations;
using GFS.Core.Enums;

namespace GFS.Transfer.Furnitures.Commands
{
    public class CreatePentagonCornerBottom
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public float Height { get; set; }
        [Required]
        public float Width1 { get; set; }
        [Required]
        public float Width2 { get; set; }
        [Required]
        public float Depth1 { get; set; }
        [Required]
        public float Depth2 { get; set; }
        [Required]
        [Range(0, 5, ErrorMessage = "Ilość półek musi być z przedziału {1} - {2}")]
        public int ShelfCount { get; set; }
      
        [Required]
        public int OrderId { get; set; }
        [Required]
        public FurnitureType FurnitureType { get; set; }

        [Required]
        public int CorpusFabricId { get; set; }
        [Required]
        public int FrontFabricId { get; set; }
        [Required]
        public int BackFabricId { get; set; }
    }
}