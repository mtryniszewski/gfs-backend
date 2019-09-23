using GFS.Core.Enums;

namespace GFS.Transfer.User.Data
{
    public class UserBasicDto
    {
        public string Id { get; set; }

        public Permissions Permissions { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchival { get; set; }
    }
}