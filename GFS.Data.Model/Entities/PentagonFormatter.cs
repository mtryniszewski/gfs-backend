using System.ComponentModel.DataAnnotations;

namespace GFS.Data.Model.Entities
{
    public class PentagonFormatter : BaseFormatter
    {
        //Formatka pięciokątna wygląda jak prostokątna, tylko ma ścięty 1 róg
    

        [Required]
        public float Width1 { get; set; }

        [Required]
        public float Width2 { get; set; }

        [Required]
        public float Depth1 { get; set; }

        [Required]
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