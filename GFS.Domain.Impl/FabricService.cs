using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using GFS.Core;
using GFS.Core.Enums;
using GFS.Data.EFCore;
using GFS.Data.Model.Entities;
using GFS.Domain.Core;
using GFS.Transfer.Fabric.Commands;
using GFS.Transfer.Fabric.Data;
using GFS.Transfer.Fabric.Queries;
using GFS.Transfer.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GFS.Domain.Impl
{
    public class FabricService : IFabricService
    {
        private readonly GfsDbContext _context;
        private readonly Dictionary _dictionary;

        public FabricService(GfsDbContext context, IOptions<Dictionary> dictionary)
        {
            _context = context;
            _dictionary = dictionary.Value;
        }

        public async Task<FabricDto> GetAsync(GetFabricQuery query)
        {
            var fabric = await _context.Fabrics
                .ProjectTo<FabricDto>()
                .FirstOrDefaultAsync(x => x.Id == query.Id);

            if (fabric == null)
                throw new GfsException(ErrorCode.FabricNotFound,_dictionary.FabricNotFound);

            return fabric;
        }

        public async Task<PageListDto<FabricBasicDto>> ListAsync(ListFabricQuery query)
        {
            if (query.ProducerId.HasValue)
            {
                if (query.ShouldSearch())
                {
                    return await _context.Fabrics
                        .Where(x => !x.IsArchival && x.ProducerId == query.ProducerId && (query.SearchBy.Any(y => x.Name.ToLower().Contains(y.ToLower()))
                                                                                          || query.SearchBy.Any(y => x.ProducerCode.ToLower()
                                                                                              .Contains(y.ToLower()))))
                        .OrderBy(x => x.Producer.Name)
                        .ThenBy(x => x.Name)
                        .ProjectTo<FabricBasicDto>()
                        .ToPagedListAsync(query);
                }


                return await _context.Fabrics
                    .Where(x => !x.IsArchival && x.ProducerId == query.ProducerId)
                    .OrderBy(x => x.Producer.Name)
                    .ThenBy(x => x.Name)
                    .ProjectTo<FabricBasicDto>()
                    .ToPagedListAsync(query);
            }

            if (query.ShouldSearch())
            {
                return await _context.Fabrics
                    .Where(x => !x.IsArchival && (query.SearchBy.Any(y => x.Name.ToLower().Contains(y.ToLower()))
                                                                                      || query.SearchBy.Any(y => x.ProducerCode.ToLower()
                                                                                          .Contains(y.ToLower()))))
                    .OrderBy(x => x.Producer.Name)
                    .ThenBy(x => x.Name)
                    .ProjectTo<FabricBasicDto>()
                    .ToPagedListAsync(query);
            }


            return await _context.Fabrics
                .Where(x => !x.IsArchival )
                .OrderBy(x => x.Producer.Name)
                .ThenBy(x => x.Name)
                .ProjectTo<FabricBasicDto>()
                .ToPagedListAsync(query);

        }

        public async Task<PageListDto<FabricBasicDto>> ListArchivesAsync(ListFabricQuery query)
        {
            if (query.ProducerId.HasValue)
            {
                if (query.ShouldSearch())
                {
                    return await _context.Fabrics
                        .Where(x => x.IsArchival && x.ProducerId == query.ProducerId && (query.SearchBy.Any(y => x.Name.ToLower().Contains(y.ToLower()))
                                                                                          || query.SearchBy.Any(y => x.ProducerCode.ToLower()
                                                                                              .Contains(y.ToLower()))))
                        .OrderBy(x => x.Producer.Name)
                        .ThenBy(x => x.ProducerCode)
                        .ProjectTo<FabricBasicDto>()
                        .ToPagedListAsync(query);
                }


                return await _context.Fabrics
                    .Where(x => x.IsArchival && x.ProducerId == query.ProducerId)
                    .OrderBy(x => x.Producer.Name)
                    .ThenBy(x => x.ProducerCode)
                    .ProjectTo<FabricBasicDto>()
                    .ToPagedListAsync(query);
            }

            if (query.ShouldSearch())
            {
                return await _context.Fabrics
                    .Where(x => x.IsArchival && (query.SearchBy.Any(y => x.Name.ToLower().Contains(y.ToLower()))
                                                  || query.SearchBy.Any(y => x.ProducerCode.ToLower()
                                                      .Contains(y.ToLower()))))
                    .OrderBy(x => x.Producer.Name)
                    .ThenBy(x => x.ProducerCode)
                    .ProjectTo<FabricBasicDto>()
                    .ToPagedListAsync(query);
            }


            return await _context.Fabrics
                .Where(x => x.IsArchival)
                .OrderBy(x => x.Producer.Name)
                .ThenBy(x => x.ProducerCode)
                .ProjectTo<FabricBasicDto>()
                .ToPagedListAsync(query);

        }

        public async Task<int> CreateAsync(CreateFabricCommand command)
        {
            var producer = await _context.Producers.FirstOrDefaultAsync(x => x.Id == command.ProducerId);
            if (producer == null)
                throw new GfsException(ErrorCode.ProducerNotFound,_dictionary.ProducerNotFound);

            var fabric = new Fabric
            {
                Thickness = command.Thickness,
                Name = command.Name,
                ImageUrl = command.ImageUrl,
                ProducerCode = command.ProducerCode,
                Producer = producer,
                ProducerId = command.ProducerId
            };

            await _context.Fabrics.AddAsync(fabric);
            await _context.SaveChangesAsync();
            return fabric.Id;
        }

        public async Task UpdateAsync(UpdateFabricCommand command)
        {
            var fabric = await _context.Fabrics.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (fabric == null)
                throw new GfsException(ErrorCode.FabricNotFound, _dictionary.FabricNotFound);

            var producer = await _context.Producers.FirstOrDefaultAsync(x => x.Id == command.ProducerId);
            if (producer == null)
                throw new GfsException(ErrorCode.ProducerNotFound, _dictionary.ProducerNotFound);

            fabric.Thickness = command.Thickness;
            fabric.Name = command.Name;
            fabric.ImageUrl = command.ImageUrl;
            fabric.ProducerCode = command.ProducerCode;
            fabric.ProducerId = producer.Id;
            fabric.Producer = producer;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeleteFabricCommand command)
        {
            var fabric = await _context.Fabrics.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (fabric == null)
                throw new GfsException(ErrorCode.FabricNotFound, _dictionary.FabricNotFound);

            _context.Fabrics.Remove(fabric);
            await _context.SaveChangesAsync();
        }

        public async Task ArchiveAsync(ManageFabricCommand command)
        {
            var fabric = await _context.Fabrics.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (fabric == null)
                throw new GfsException(ErrorCode.FabricNotFound, _dictionary.FabricNotFound);

            fabric.IsArchival = true;
            await _context.SaveChangesAsync();
        }

        public async Task DearchiveAsync(ManageFabricCommand command)
        {
            var fabric = await _context.Fabrics.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (fabric == null)
                throw new GfsException(ErrorCode.FabricNotFound, _dictionary.FabricNotFound);

            fabric.IsArchival = false;
            await _context.SaveChangesAsync();
        }
    }
}