using System;
using System.Collections.Generic;
using System.Text;

namespace GFS.Transfer.Formatters.Data
{
    public class TriangularFormatterDto : BaseFormatterDto
    {
        public float HypotenuseLength { get; set; }
        public float Width { get; set; }
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
