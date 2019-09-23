using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using GFS.Core;
using GFS.Core.Enums;
using GFS.Data.EFCore;
using GFS.Data.Model.Entities;
using GFS.Domain.Core;
using GFS.Transfer.Producer.Commands;
using GFS.Transfer.Producer.Data;
using GFS.Transfer.Producer.Queries;
using GFS.Transfer.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GFS.Domain.Impl
{
    public class ProducerService : IProducerService
    {
        private readonly GfsDbContext _context;
        private readonly Dictionary _dictionary;
        public ProducerService(GfsDbContext context,IOptions<Dictionary> dictionary)
        {
            _context = context;
            _dictionary = dictionary.Value;
        }

        public async Task<ProducerDto> GetAsync(GetProducerQuery query)
        {
            var producer = await _context.Producers
                .ProjectTo<ProducerDto>()
                .FirstOrDefaultAsync(x => x.Id == query.Id);

            if (producer == null)
                throw new GfsException(ErrorCode.ProducerNotFound,_dictionary.ProducerNotFound);

            return producer;
        }

        public async Task<PageListDto<ProducerBasicDto>> ListAsync(ListProducerQuery query)
        {
            if (query.ShouldSearch())
                return await _context.Producers
                    .Where(x => !x.IsArchival && query.SearchBy.Any(y => x.Name.ToLower().Contains(y.ToLower())))
                    .ProjectTo<ProducerBasicDto>()
                    .OrderBy(x => x.Name)
                    .ToPagedListAsync(query);

            return await _context.Producers
                .Where(x => !x.IsArchival)
                .ProjectTo<ProducerBasicDto>()
                .OrderBy(x => x.Name)
                .ToPagedListAsync(query);
        }
        public async Task<PageListDto<ProducerSimpleDto>> ListSimpleAsync(ListProducerQuery query)
        {
            if (query.ShouldSearch())
                return await _context.Producers
                    .Where(x => !x.IsArchival && query.SearchBy.Any(y => x.Name.ToLower().Contains(y.ToLower())))
                    .ProjectTo<ProducerSimpleDto>()
                    .OrderBy(x => x.Name)
                    .ToPagedListAsync(query);

            return await _context.Producers
                .Where(x => !x.IsArchival)
                .ProjectTo<ProducerSimpleDto>()
                .OrderBy(x => x.Name)
                .ToPagedListAsync(query);
        }
        public async Task<PageListDto<ProducerBasicDto>> ListArchivesAsync(ListProducerQuery query)
        {
            if (query.ShouldSearch())
                return await _context.Producers
                    .Where(x => x.IsArchival && query.SearchBy.Any(y => x.Name.ToLower().Contains(y.ToLower())))
                    .ProjectTo<ProducerBasicDto>()
                    .OrderBy(x => x.Name)
                    .ToPagedListAsync(query);

            return await _context.Producers
                .Where(x => x.IsArchival)
                .ProjectTo<ProducerBasicDto>()
                .OrderBy(x => x.Name)
                .ToPagedListAsync(query);
        }

        public async Task<int> CreateAsync(CreateProducerCommand command)
        {
            var producer = new Producer
            {
                Name = command.Name,
                Street = command.Street,
                City = command.City,
                Email = command.Email,
                PhoneNumber = command.PhoneNumber
            };

            await _context.Producers.AddAsync(producer);
            await _context.SaveChangesAsync();
            return producer.Id;
        }

        public async Task UpdateAsync(UpdateProducerCommand command)
        {
            var producer = await _context.Producers
                .FirstOrDefaultAsync(x => x.Id == command.Id);

            if (producer == null)
                throw new GfsException(ErrorCode.ProducerNotFound, _dictionary.ProducerNotFound);

            producer.Name = command.Name;
            producer.City = command.City;
            producer.Street = command.Street;
            producer.Email = command.Email;
            producer.PhoneNumber = command.PhoneNumber;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeleteProducerCommand command)
        {
            var producer = await _context.Producers.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (producer == null)
                throw new GfsException(ErrorCode.ProducerNotFound, _dictionary.ProducerNotFound);

            _context.Producers.Remove(producer);
            await _context.SaveChangesAsync();
        }

        public async Task ArchiveAsync(ManageProducerCommand command)
        {
            var producer = await _context.Producers.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (producer == null)
                throw new GfsException(ErrorCode.ProducerNotFound, _dictionary.ProducerNotFound);

            producer.IsArchival = true;
            await _context.SaveChangesAsync();
        }

        public async Task DearchiveAsync(ManageProducerCommand command)
        {
            var producer = await _context.Producers.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (producer == null)
                throw new GfsException(ErrorCode.ProducerNotFound, _dictionary.ProducerNotFound);

            producer.IsArchival = false;
            await _context.SaveChangesAsync();
        }
    }
}