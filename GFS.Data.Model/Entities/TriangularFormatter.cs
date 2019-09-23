using System.ComponentModel.DataAnnotations;

namespace GFS.Data.Model.Entities
{
    public class TriangularFormatter : BaseFormatter
    {
        [Required]
        public float HypotenuseLength { get; set; }
        [Required]
        public float Width { get; set; }
        [Required]
        public float Length { get; set; }
        //Obramowanie
        public bool IsBottomBorder { get; set; }
        public bool IsHypotenuseBorder { get; set; }
        public bool IsLeftBorder { get; set; }
        public float? BottomBorderThickness { get; set; }
        public float? HypotenuseBorderThickness { get; set; }
        public float? LeftBorderThickness { get; set; }
    }
}