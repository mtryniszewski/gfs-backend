using System.ComponentModel.DataAnnotations;
using GFS.Core.Enums;

namespace GFS.Transfer.Furnitures.Commands
{
    public class CreateOnlyDrawersBottomCommand
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public float Height { get; set; }

        [Required]
        public float Width { get; set; }

        [Required]
        public float Depth { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public FurnitureType FurnitureType { get; set; }

        [Required]
        public DrawerConfiguration DrawerConfiguration { get; set; }

        [Required]
        public DrawerType DrawerType { get; set; }

        [Required]
        public int CorpusFabricId { get; set; }

        [Required]
        public int FrontFabricId { get; set; }

        [Required]
        public int BackFabricId { get; set; }
    }
}