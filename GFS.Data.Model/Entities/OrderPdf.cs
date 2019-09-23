using System;
using System.Collections.Generic;
using System.Text;
using GFS.Transfer.Order.Data;

namespace GFS.Data.Model.Entities
{
    public class OrderPdf : OrderDetailsDto
    {
        //Dane składającego
        public string Name { get; set; }
        public string Surname { get; set; }
        //Dane zamówienia
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
