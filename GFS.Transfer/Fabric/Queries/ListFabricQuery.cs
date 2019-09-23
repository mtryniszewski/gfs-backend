using GFS.Transfer.Shared;

namespace GFS.Transfer.Fabric.Queries
{
    public class ListFabricQuery : ListQuery
    {
        public int? ProducerId { get; set; }
    }
}