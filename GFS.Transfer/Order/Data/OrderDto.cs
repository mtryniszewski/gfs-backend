using GFS.Transfer.User.Data;

namespace GFS.Transfer.Order.Data
{
    public class OrderDto : OrderBasicDto
    {
        public string UserId { get; set; }
        public UserDto User { get; set; }
    }
}