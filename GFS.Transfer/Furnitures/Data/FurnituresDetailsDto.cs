using System;
using System.Collections.Generic;
using System.Text;
using GFS.Transfer.Formatters.Data;

namespace GFS.Transfer.Furnitures.Data
{
    public class FurnituresDetailsDto : FurnituresDto
    {
        public List<LFormatterDto> LFormatterDtos { get; set; }
        public List<PentagonFormatterDto> PentagonFormatterDtos { get; set; }
        public List<RectangularFormatterDto> RectangularFormatterDtos { get; set; }
        public List<TriangularFormatterDto> TriangularFormatterDtos { get; set; }
    }
}
