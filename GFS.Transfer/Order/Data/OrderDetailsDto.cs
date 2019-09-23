using System;
using System.Collections.Generic;
using System.Text;
using GFS.Transfer.Furnitures.Data;

namespace GFS.Transfer.Order.Data
{
    public class OrderDetailsDto : OrderDto
    {
        public List<FurnituresDetailsDto> FurnituresDetailsDtos { get; set; }
    }
}
