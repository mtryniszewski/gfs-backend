using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using GFS.Core;
using GFS.Core.Enums;
using GFS.Data.EFCore;
using GFS.Data.Model.Entities;
using GFS.Domain.Core;
using GFS.Transfer.Formatters.Data;
using GFS.Transfer.Furnitures.Data;
using GFS.Transfer.Order.Commands;
using GFS.Transfer.Order.Data;
using GFS.Transfer.Order.Queries;
using GFS.Transfer.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GFS.Domain.Impl
{
    public class OrderService : IOrderService
    {
        private readonly GfsDbContext _context;
        private readonly Dictionary _dictionary;

        public OrderService(GfsDbContext context,IOptions<Dictionary> dictionary)
        {
            _context = context;
            _dictionary = dictionary.Value;
        }

        public async Task<OrderDto> GetAsync(GetOrderQuery query)
        {
            var order = await _context.Orders
                .ProjectTo<OrderDto>()
                .FirstOrDefaultAsync(x => x.Id == query.Id);

            if (order == null)
                throw new GfsException(ErrorCode.OrderNotFound,_dictionary.OrderNotFound);

            return order;
        }

      public async Task<PageListDto<OrderBasicDto>> ListAsync(ListOrderQuery query)
        {
            if (query.ShouldSearch())
                return await _context.Orders
                    .Where(x =>x.UserId==query.UserId&& !x.IsArchival && (query.SearchBy.Any(y => x.Description.ToLower().Contains(y.ToLower()))
                                                  ))
                    .ProjectTo<OrderBasicDto>()
                    .OrderByDescending(x => x.Date)
                    .ToPagedListAsync(query);

            return await _context.Orders
                .Where(x => x.UserId == query.UserId && !x.IsArchival)
                .ProjectTo<OrderBasicDto>()
                .OrderByDescending(x => x.Date)
                .ToPagedListAsync(query);
        }

        public async Task<OrderDetailsDto> ShowOrderDetailsAsync(GetOrderQuery query)
        {
            var details = await _context.Orders
                .ProjectTo<OrderDetailsDto>()
                .FirstOrDefaultAsync(x => x.Id == query.Id);

            foreach (var furnitures in details.FurnituresDetailsDtos)
            {
                await GetFormatters(furnitures);
            }

            return details;
        }

        public async Task<PageListDto<OrderBasicDto>> ListAllAsync(ListAllOrdersQuery query)
        {
            if (query.ShouldSearch())
                return await _context.Orders
                    .Where(x => (query.SearchBy.Any(y => x.Description.ToLower().Contains(y.ToLower()))
                                ))
                    .ProjectTo<OrderBasicDto>()
                    .OrderByDescending(x => x.Date)
                    .ToPagedListAsync(query);

            return await _context.Orders
                .ProjectTo<OrderBasicDto>()
                .Where(x=>!x.IsArchival)
                .OrderByDescending(x => x.Date)
                .ToPagedListAsync(query);
        }

        public async Task<PageListDto<OrderBasicDto>> ListArchivesAsync(ListOrderQuery query)
        {
            if (query.ShouldSearch())
                return await _context.Orders
                    .Where(x => x.UserId == query.UserId && x.IsArchival && (query.SearchBy.Any(y => x.Description.ToLower().Contains(y.ToLower()))
                                ))
                    .ProjectTo<OrderBasicDto>()
                    .OrderByDescending(x => x.Date)
                    .ToPagedListAsync(query);

            return await _context.Orders
                .Where(x => x.UserId == query.UserId && x.IsArchival)
                .ProjectTo<OrderBasicDto>()
                .OrderByDescending(x => x.Date)
                .ToPagedListAsync(query);
        }

        public async Task<PageListDto<OrderBasicDto>> ListAllArchivesAsync(ListAllOrdersQuery query)
        {
            if (query.ShouldSearch())
                return await _context.Orders
                    .Where(x => x.IsArchival && (query.SearchBy.Any(y => x.Description.ToLower().Contains(y.ToLower()))
                                ))
                    .ProjectTo<OrderBasicDto>()
                    .OrderByDescending(x => x.Date)
                    .ToPagedListAsync(query);

            return await _context.Orders
                .Where(x => x.IsArchival)
                .ProjectTo<OrderBasicDto>()
                .OrderByDescending(x => x.Date)
                .ToPagedListAsync(query);
        }
    
        public async Task<int> CreateAsync(CreateOrderCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.UserId);
            if (user == null)
                throw new GfsException(ErrorCode.UserNotFound, _dictionary.UserNotFound);

            var order = new Order
            {
                Description = command.Description,
                Date = DateTime.Now,
                IsConfirmed = false,
                User = user,
                UserId = command.UserId
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task UpdateAsync(UpdateOrderCommand command)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (order == null)
                throw new GfsException(ErrorCode.OrderNotFound, _dictionary.OrderNotFound);
            
            order.Description = command.Description;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeleteOrderCommand command)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (order == null)
                throw new GfsException(ErrorCode.OrderNotFound, _dictionary.OrderNotFound);

            _context.Remove(order);

            await _context.SaveChangesAsync();
        }

        public async Task ConfirmAsync(ManageOrderCommand command)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == command.Id);

            if (order == null)
                throw new GfsException(ErrorCode.OrderNotFound, _dictionary.OrderNotFound);

            order.IsConfirmed = true;

            await _context.SaveChangesAsync();
        }
        public async Task ArchiveAsync(ManageOrderCommand command)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == command.Id);

            if (order == null)
                throw new GfsException(ErrorCode.OrderNotFound, _dictionary.OrderNotFound);

            order.IsArchival = true;

            await _context.SaveChangesAsync();
        }

        public async Task DearchiveAsync(ManageOrderCommand command)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == command.Id);

            if (order == null)
                throw new GfsException(ErrorCode.OrderNotFound, _dictionary.OrderNotFound);

            order.IsArchival = false;

            await _context.SaveChangesAsync();
        }

        private async Task GetFormatters(FurnituresDetailsDto furniture)
        {

            furniture.LFormatterDtos=await _context.LFormatters.ProjectTo<LFormatterDto>().Where(x=>x.FurnitureId==furniture.Id).ToListAsync();
            furniture.RectangularFormatterDtos=await _context.RectangularFormatters.ProjectTo<RectangularFormatterDto>().Where(x=>x.FurnitureId==furniture.Id).ToListAsync();
            furniture.TriangularFormatterDtos=await _context.TriangularFormatters.ProjectTo<TriangularFormatterDto>().Where(x=>x.FurnitureId==furniture.Id).ToListAsync();
            furniture.PentagonFormatterDtos=await _context.PentagonFormatters.ProjectTo<PentagonFormatterDto>().Where(x=>x.FurnitureId==furniture.Id).ToListAsync();

        }
    }
}