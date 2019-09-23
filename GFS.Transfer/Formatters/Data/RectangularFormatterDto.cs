using System;
using System.Collections.Generic;
using System.Text;

namespace GFS.Transfer.Formatters.Data
{
    public class RectangularFormatterDto : BaseFormatterDto
    {
        public float Width { get; set; }
        public float Length { get; set; }
        //Obramowanie
        public bool IsTopBorder { get; set; }
        public bool IsBottomBorder { get; set; }
        public bool IsRightBorder { get; set; }
        public bool IsLeftBorder { get; set; }
        public float? TopBorderThickness { get; set; }
        public float? BottomBorderThickness { get; set; }
        public float? RightBorderThickness { get; set; }
        public float? LeftBorderThickness { get; set; }
    }
}
