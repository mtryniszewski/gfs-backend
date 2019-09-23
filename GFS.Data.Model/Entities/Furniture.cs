using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GFS.Core.Enums;
using GFS.Data.Model.Entities;

namespace GFS.Data.Model.Entities
{
    public class Furniture
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public FurnitureType FurnitureType { get; set; }
        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public List<RectangularFormatter>RectangularFormatters { get; set; }
        public List<TriangularFormatter> TriangularFormatters { get; set; }
        public List<PentagonFormatter> PentagonFormatters { get; set; }
        public List<LFormatter> LFormatters { get; set; }
    }
}