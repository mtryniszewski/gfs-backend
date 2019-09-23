using System.Threading.Tasks;
using GFS.Transfer.Fabric.Commands;
using GFS.Transfer.Fabric.Data;
using GFS.Transfer.Fabric.Queries;
using GFS.Transfer.Shared;

namespace GFS.Domain.Core
{
    public interface IFabricService
    {
        Task<FabricDto> GetAsync(GetFabricQuery query);
        Task<PageListDto<FabricBasicDto>> ListAsync(ListFabricQuery query);
        Task<PageListDto<FabricBasicDto>> ListArchivesAsync(ListFabricQuery query);
        Task<int> CreateAsync(CreateFabricCommand command);
        Task UpdateAsync(UpdateFabricCommand command);
        Task DeleteAsync(DeleteFabricCommand command);
        Task ArchiveAsync(ManageFabricCommand command);
        Task DearchiveAsync(ManageFabricCommand command);
    }
}