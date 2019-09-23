using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Producer.Commands
{
    public class UpdateProducerCommand
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}