using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GFS.Data.Model.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string UserId { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsArchival { get; set; }
        public string Description { get; set; }


        public User User { get; set; }
        public List<Furniture> Furnitures { get; set; }
    }
}