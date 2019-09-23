using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GFS.Core.Enums;
using GFS.Transfer.Order.Data;

namespace GFS.Transfer.Furnitures.Data
{
    public class FurnituresDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FurnitureType FurnitureType { get; set; }
        public int OrderId { get; set; }
        public OrderDto Order { get; set; }
    }
}
