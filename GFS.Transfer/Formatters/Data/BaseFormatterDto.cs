using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GFS.Core.Enums;
using GFS.Transfer.Fabric.Data;
using GFS.Transfer.Furnitures.Data;

namespace GFS.Transfer.Formatters.Data
{
    public abstract class BaseFormatterDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Count { get; set; }

        //Frezowanie
        public bool IsMilling { get; set; }
        public Milling? Milling { get; set; }
        public float? CutterLength { get; set; }
        public float? CutterWidth { get; set; }
        public float? CutterDepth { get; set; }
        public float? TopSpace { get; set; }
        public float? LeftSpace { get; set; }

        //Wymiary

        [Required]
        public float Thickness { get; set; }

        [Required]
        public int FurnitureId { get; set; }
        public FurnituresDto Furniture { get; set; }
        [Required]
        public int FabricId { get; set; }
        public FabricDto Fabric { get; set; }
    }
}
