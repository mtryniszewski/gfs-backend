using System.ComponentModel.DataAnnotations;
using GFS.Core.Enums;

namespace GFS.Data.Model.Entities
{
    public abstract class BaseFormatter
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
        public Furniture Furniture { get; set; }
        [Required]
        public int FabricId { get; set; }
        public Fabric Fabric { get; set; }

    }
}