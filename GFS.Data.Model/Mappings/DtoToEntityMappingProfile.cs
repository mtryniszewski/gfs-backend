using System.Runtime.ExceptionServices;
using AutoMapper;
using GFS.Data.Model.Entities;
using GFS.Transfer.Fabric.Data;
using GFS.Transfer.Formatters.Data;
using GFS.Transfer.Furnitures.Data;
using GFS.Transfer.Order.Data;
using GFS.Transfer.Producer.Data;
using GFS.Transfer.User.Commands;

namespace GFS.Data.Model.Mappings
{
    public class DtoToEntityMappingProfile : Profile
    {
        public DtoToEntityMappingProfile()
        {
            CreateMap<CreateUserCommand, User>().ForMember(user => user.UserName, map => map.MapFrom(dto => dto.Email));
            CreateMap<Fabric, FabricDto>();
            CreateMap<LFormatter, LFormatterDto>();
            CreateMap<RectangularFormatter, RectangularFormatterDto>();
            CreateMap<PentagonFormatter, PentagonFormatterDto>();
            CreateMap<TriangularFormatter, TriangularFormatterDto>();
            CreateMap<Producer, ProducerDto>();
            CreateMap<Order, OrderDetailsDto>().ForMember(dto=>dto.FurnituresDetailsDtos, map=>map.MapFrom(order=> order.Furnitures));
            CreateMap<Furniture, FurnituresDetailsDto>().ForMember(dto=>dto.LFormatterDtos,map=>map.MapFrom(fur=>fur.LFormatters));
            CreateMap<Furniture, FurnituresDetailsDto>().ForMember(dto=>dto.RectangularFormatterDtos,map=>map.MapFrom(fur=>fur.RectangularFormatters));
            CreateMap<Furniture, FurnituresDetailsDto>().ForMember(dto=>dto.PentagonFormatterDtos,map=>map.MapFrom(fur=>fur.PentagonFormatters));
            CreateMap<Furniture, FurnituresDetailsDto>().ForMember(dto=>dto.TriangularFormatterDtos,map=>map.MapFrom(fur=>fur.TriangularFormatters));
        }
    }
}