using System.ComponentModel.DataAnnotations;

namespace GFS.Data.Model.Entities
{
    public class RectangularFormatter:BaseFormatter
    {
        [Required]
        public float Width { get; set; }
        [Required]
        public float Length { get; set; }
        //Obramowanie
        public bool IsTopBorder { get; set; }
        public bool IsBottomBorder { get; set; }
        public bool  IsRightBorder { get; set; }
        public bool IsLeftBorder { get; set; }
        public float? TopBorderThickness { get; set; }
        public float? BottomBorderThickness { get; set; }
        public float? RightBorderThickness { get; set; }
        public float? LeftBorderThickness { get; set; }
    }
}