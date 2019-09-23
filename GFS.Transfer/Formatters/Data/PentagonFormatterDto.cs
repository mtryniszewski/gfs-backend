using System;
using System.Collections.Generic;
using System.Text;

namespace GFS.Transfer.Formatters.Data
{
    public class PentagonFormatterDto : BaseFormatterDto
    {
        //Formatka pięciokątna wygląda jak prostokątna, tylko ma ścięty 1 róg


        public float Width1 { get; set; }
        public float Width2 { get; set; }

        public float Depth1 { get; set; }
        public float Depth2 { get; set; }


        //Obramowanie
        public bool IsBottomBorder { get; set; }

        public bool IsHypotenuseBorder { get; set; }
        public bool IsLeftBorder { get; set; }
        public bool IsTopBorder { get; set; }
        public bool IsRightBorder { get; set; }

        public float? BottomBorderThickness { get; set; }
        public float? HypotenuseBorderThickness { get; set; }
        public float? LeftBorderThickness { get; set; }
        public float? TopBorderThickness { get; set; }
        public float? RightBorderThickness { get; set; }
    }
}
