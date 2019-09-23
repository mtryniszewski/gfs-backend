using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GFS.Core.Enums;

namespace GFS.Transfer.Furnitures.Commands
{
    public class CreateSingleFormatterCommand
    {
        [Required] public string Name { get; set; }

        [Required] public float Length { get; set; }
        [Required] public float Width { get; set; }
        [Required] public int OrderId { get; set; }
        [Required] public FurnitureType FurnitureType { get; set; }

        [Required] public int CorpusFabricId { get; set; }
    }
}
