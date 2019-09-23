using System;
using System.Collections.Generic;
using System.Text;

namespace GFS.Transfer.Formatters.Data
{
    public class LFormatterDto : BaseFormatterDto
    {
        public float Width1 { get; set; }
        public float Width2 { get; set; }
        public float Depth1 { get; set; }
        public float Depth2 { get; set; }
        public float Indentation1 { get; set; }
        public float Indentation2 { get; set; }
        //Obramowanie
        public bool IsWidth1Border { get; set; }
        public bool IsWidth2Border { get; set; }
        public bool IsDepth1Border { get; set; }
        public bool IsDepth2Border { get; set; }
        public bool IsIndentation1Border { get; set; }
        public bool IsIndentation2Border { get; set; }
        public float? Width1BorderThickness { get; set; }
        public float? Width2BorderThickness { get; set; }
        public float? Depth1BorderThickness { get; set; }
        public float? Depth2BorderThickness { get; set; }
        public float? Indentation1BorderThickness { get; set; }
        public float? Indentation2BorderThickness { get; set; }
    }
}
