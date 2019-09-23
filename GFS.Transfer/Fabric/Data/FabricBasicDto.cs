using GFS.Transfer.Producer.Data;

namespace GFS.Transfer.Fabric.Data
{
    public class FabricBasicDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProducerCode { get; set; }
        public float Thickness { get; set; }
        public string ImageUrl { get; set; }
        public ProducerDto Producer { get; set; }
        public string ProducerId { get; set; }
    }
}