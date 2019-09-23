using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GFS.Data.Model.Entities;

namespace GFS.Data.Model.Entities
{
    public class Fabric
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ProducerCode { get; set; }

        [Required]
        public int ProducerId { get; set; }

        public float Thickness { get; set; }
        public bool IsArchival { get; set; }
        public string ImageUrl { get; set; }
        public Producer Producer { get; set; }

        public List<RectangularFormatter> RectangularFormatters { get; set; }
        public List<TriangularFormatter> TriangularFormatters { get; set; }
        public List<PentagonFormatter> PentagonFormatters { get; set; }
        public List<LFormatter> LFormatters { get; set; }
    }
}