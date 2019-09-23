using System.Threading.Tasks;
using GFS.Transfer.Order.Commands;
using GFS.Transfer.Order.Data;
using GFS.Transfer.Order.Queries;
using GFS.Transfer.Shared;

namespace GFS.Domain.Core
{
    public interface IOrderService
    {
        Task<OrderDto> GetAsync(GetOrderQuery query);
        Task<PageListDto<OrderBasicDto>> ListAsync(ListOrderQuery query);
        Task<PageListDto<OrderBasicDto>> ListArchivesAsync(ListOrderQuery query);
        Task<PageListDto<OrderBasicDto>> ListAllAsync(ListAllOrdersQuery query);
        Task<PageListDto<OrderBasicDto>> ListAllArchivesAsync(ListAllOrdersQuery query);
        Task<OrderDetailsDto> ShowOrderDetailsAsync(GetOrderQuery query);
        Task<int> CreateAsync(CreateOrderCommand command);
        Task UpdateAsync(UpdateOrderCommand command);
        Task DeleteAsync(DeleteOrderCommand command);
        Task ConfirmAsync(ManageOrderCommand command);
        Task ArchiveAsync(ManageOrderCommand command);
        Task DearchiveAsync(ManageOrderCommand command);
    }
}