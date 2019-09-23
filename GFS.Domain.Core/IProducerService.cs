using System.Threading.Tasks;
using GFS.Transfer.Producer.Commands;
using GFS.Transfer.Producer.Data;
using GFS.Transfer.Producer.Queries;
using GFS.Transfer.Shared;

namespace GFS.Domain.Core
{
    public interface IProducerService
    {
        Task<ProducerDto> GetAsync(GetProducerQuery query);
        Task<PageListDto<ProducerBasicDto>> ListAsync(ListProducerQuery query);
        Task<PageListDto<ProducerSimpleDto>> ListSimpleAsync(ListProducerQuery query);
        Task<PageListDto<ProducerBasicDto>> ListArchivesAsync(ListProducerQuery query);
        Task<int> CreateAsync(CreateProducerCommand command);
        Task UpdateAsync(UpdateProducerCommand command);
        Task DeleteAsync(DeleteProducerCommand command);
        Task ArchiveAsync(ManageProducerCommand command);
        Task DearchiveAsync(ManageProducerCommand command);
    }
}