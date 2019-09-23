using System;
using GFS.Transfer.User.Data;

namespace GFS.Transfer.Order.Data
{
    public class OrderBasicDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsArchival { get; set; }
        public UserBasicDto User { get; set; }
    }
}