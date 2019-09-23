using System.Threading.Tasks;
using GFS.Core.Enums;
using GFS.Data.Model.Entities;

namespace GFS.Domain.Core
{
    public interface IDrawerService
    {
        Task CreateOnlyHighAsync(DrawerType drawerType, float height, float width, float depth,
            Fabric corpusFabric, Fabric frontFabric, Furniture furniture, int count);


        Task CreateMixedConfigAsync(DrawerType drawerType, float height, float width,
            float depth, Fabric corpusFabric, Fabric frontFabric, Furniture furniture, int shortCount, int highCount);


        Task CreateOnlyShortAsync(DrawerType drawerType, float height, float width, float depth,
            Fabric corpusFabric, Fabric frontFabric, Furniture furniture, int count);

        Task CreateVersaliteDrawersAsync(float width, float depth, float frontHeight, float drawerHeight,
            Fabric corpusFabric, Fabric frontFabric, int count, Furniture furniture);

        Task CreateAmixBlumDrawersAsync(float width, float depth, float frontHeight, float drawerHeight,
            Fabric corpusFabric, Fabric frontFabric, int count, Furniture furniture);
    }
}