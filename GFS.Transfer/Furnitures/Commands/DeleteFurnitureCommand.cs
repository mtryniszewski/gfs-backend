using System.ComponentModel.DataAnnotations;

namespace GFS.Transfer.Furnitures.Commands
{
    public class DeleteFurnitureCommand
    {
        [Required] public int Id { get; set; }
    }
}