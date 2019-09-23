using System.Collections.Generic;

namespace GFS.Data.Model.Entities
{
    public class Producer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsArchival { get; set; }
        public List<Fabric> Fabrics { get; set; }

    }
}