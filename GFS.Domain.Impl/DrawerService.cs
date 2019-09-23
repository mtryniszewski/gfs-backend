using System.Threading.Tasks;
using GFS.Core;
using GFS.Core.Enums;
using GFS.Data.EFCore;
using GFS.Data.Model.Entities;
using GFS.Domain.Core;
using Microsoft.Extensions.Options;

namespace GFS.Domain.Impl
{
    public class DrawerService : IDrawerService
    {
        private readonly GfsDbContext _context;
        private readonly Dictionary _dictionary;
        public DrawerService(GfsDbContext context, IOptions<Dictionary> dictionary)
        {
            _context = context;
            _dictionary = dictionary.Value;
        }

        public async Task CreateOnlyHighAsync(DrawerType drawerType, float height, float width, float depth,
            Fabric corpusFabric, Fabric frontFabric, Furniture furniture, int count)
        {
            if (drawerType == DrawerType.Versalite)
                await CreateVersaliteDrawersAsync(width, depth, (height - count * 4) / count, 220, corpusFabric,
                    frontFabric, count,
                    furniture);
            else if (drawerType == DrawerType.AmixBlum)
                await CreateAmixBlumDrawersAsync(width, depth, (height - count * 4) / count, 200, corpusFabric,
                    frontFabric, count,
                    furniture);
            else
                throw new GfsException(ErrorCode.WrongDrawerType, _dictionary.WrongDrawerType);
        }

        public async Task CreateMixedConfigAsync(DrawerType drawerType, float height, float width,
            float depth, Fabric corpusFabric, Fabric frontFabric, Furniture furniture, int shortCount, int highCount)
        {
            if (drawerType == DrawerType.Versalite)
            {
                await CreateVersaliteDrawersAsync(width, depth, 284, 220, corpusFabric, frontFabric, highCount,
                    furniture);
                await CreateVersaliteDrawersAsync(width, depth, 140, 90, corpusFabric, frontFabric, shortCount,
                    furniture);
            }
            else if (drawerType == DrawerType.AmixBlum)
            {
                await CreateAmixBlumDrawersAsync(width, depth, 284, 200, corpusFabric, frontFabric, highCount,
                    furniture);

                await CreateAmixBlumDrawersAsync(width, depth, 140, 90, corpusFabric, frontFabric, shortCount,
                    furniture);
            }
            else
            {
                throw new GfsException(ErrorCode.WrongDrawerType,_dictionary.WrongDrawerType);
            }
        }

        public async Task CreateOnlyShortAsync(DrawerType drawerType, float height, float width, float depth,
            Fabric corpusFabric, Fabric frontFabric, Furniture furniture, int count)
        {
            if (drawerType == DrawerType.Versalite)
                await CreateVersaliteDrawersAsync(width, depth, (height - count * 4) / count, 90, corpusFabric,
                    frontFabric, count,
                    furniture);
            else if (drawerType == DrawerType.AmixBlum)
                await CreateAmixBlumDrawersAsync(width, depth, (height - count * 4) / count, 90, corpusFabric,
                    frontFabric, count,
                    furniture);
            else
                throw new GfsException(ErrorCode.WrongDrawerType,_dictionary.WrongDrawerType);
        }


        public async Task CreateVersaliteDrawersAsync(float width, float depth, float frontHeight, float drawerHeight,
            Fabric corpusFabric, Fabric frontFabric, int count, Furniture furniture)
        {
            await _context.RectangularFormatters.AddAsync(new RectangularFormatter
            {
                Name = "BOK LEWY SZUFLADY",
                Count = count,
                IsMilling = false,
                Width = depth - 20,
                Length = drawerHeight,
                Thickness = corpusFabric.Thickness,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                Fabric = corpusFabric,
                FabricId = corpusFabric.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsRightBorder = false,
                IsLeftBorder = true,
                LeftBorderThickness = 1
            });
            await _context.RectangularFormatters.AddAsync(new RectangularFormatter
            {
                Name = "BOK PRAWY SZUFLADY",
                Count = count,
                IsMilling = false,
                Width = depth - 20,
                Length = drawerHeight,
                Thickness = corpusFabric.Thickness,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                Fabric = corpusFabric,
                FabricId = corpusFabric.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsRightBorder = true,
                IsLeftBorder = false,
                RightBorderThickness = 1
            });
            await _context.RectangularFormatters.AddAsync(new RectangularFormatter
            {
                Name = "PRZÓD SZUFLADY",
                Count = count,
                IsMilling = false,
                Width = width - 2 * corpusFabric.Thickness - 27 - 2 * corpusFabric.Thickness,
                Length = drawerHeight,
                Thickness = corpusFabric.Thickness,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                Fabric = corpusFabric,
                FabricId = corpusFabric.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsRightBorder = false,
                IsLeftBorder = true,
                LeftBorderThickness = 1
            });
            await _context.RectangularFormatters.AddAsync(new RectangularFormatter
            {
                Name = "TYŁ SZUFLADY",
                Count = count,
                IsMilling = false,
                Width = width - 2 * corpusFabric.Thickness - 27 - 2 * corpusFabric.Thickness,
                Length = drawerHeight,
                Thickness = corpusFabric.Thickness,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                Fabric = corpusFabric,
                FabricId = corpusFabric.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsRightBorder = true,
                IsLeftBorder = false,
                RightBorderThickness = 1
            });
            await _context.RectangularFormatters.AddAsync(new RectangularFormatter
            {
                Name = "DNO SZUFLADY",
                Count = count,
                IsMilling = false,
                Width = width - 2 * corpusFabric.Thickness - 27,
                Length = depth - 20,
                Thickness = corpusFabric.Thickness,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                Fabric = corpusFabric,
                FabricId = corpusFabric.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsRightBorder = false,
                IsLeftBorder = false
            });
            await _context.RectangularFormatters.AddAsync(new RectangularFormatter
            {
                Name = "FRONT SZUFLADY",
                Count = count,
                IsMilling = false,
                Width = width - 4,
                Length = frontHeight,
                Thickness = corpusFabric.Thickness,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                Fabric = frontFabric,
                FabricId = frontFabric.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                LeftBorderThickness = 1,
                RightBorderThickness = 1
            });

            await _context.SaveChangesAsync();
        }

        public async Task CreateAmixBlumDrawersAsync(float width, float depth, float frontHeight, float drawerHeight,
            Fabric corpusFabric, Fabric frontFabric, int count, Furniture furniture)
        {
            await _context.RectangularFormatters.AddAsync(new RectangularFormatter
            {
                Name = "BOK LEWY SZUFLADY",
                Count = count,
                IsMilling = true,
                Milling = Milling.LengthMilling,
                CutterLength = depth - 30,
                CutterWidth = 10.5F,
                CutterDepth = 8,
                LeftSpace = drawerHeight - 22,
                TopSpace = 0,
                Width = depth - 30,
                Length = drawerHeight,
                Thickness = corpusFabric.Thickness,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                Fabric = corpusFabric,
                FabricId = corpusFabric.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsRightBorder = false,
                IsLeftBorder = true,
                LeftBorderThickness = 1
            });
            await _context.RectangularFormatters.AddAsync(new RectangularFormatter
            {
                Name = "BOK PRAWY SZUFLADY",
                Count = count,
                IsMilling = true,
                Milling = Milling.LengthMilling,
                CutterLength = depth - 30,
                CutterWidth = 10.5F,
                CutterDepth = 8,
                LeftSpace = 11.5F,
                TopSpace = 0,
                Width = depth - 30,
                Length = drawerHeight,
                Thickness = corpusFabric.Thickness,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                Fabric = corpusFabric,
                FabricId = corpusFabric.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsRightBorder = true,
                IsLeftBorder = false,
                RightBorderThickness = 1
            });
            await _context.RectangularFormatters.AddAsync(new RectangularFormatter
            {
                Name = "PRZÓD SZUFLADY",
                Count = count,
                IsMilling = false,
                Width = width - 2 * corpusFabric.Thickness - 46,
                Length = drawerHeight,
                Thickness = corpusFabric.Thickness,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                Fabric = corpusFabric,
                FabricId = corpusFabric.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsRightBorder = false,
                IsLeftBorder = true,
                LeftBorderThickness = 1
            });
            await _context.RectangularFormatters.AddAsync(new RectangularFormatter
            {
                Name = "TYŁ SZUFLADY",
                Count = count,
                IsMilling = false,
                Width = width - 2 * corpusFabric.Thickness - 46,
                Length = drawerHeight,
                Thickness = corpusFabric.Thickness,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                Fabric = corpusFabric,
                FabricId = corpusFabric.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsRightBorder = true,
                IsLeftBorder = false,
                RightBorderThickness = 1
            });
            await _context.RectangularFormatters.AddAsync(new RectangularFormatter
            {
                Name = "DNO SZUFLADY",
                Count = count,
                IsMilling = false,
                Width = width - 2 * corpusFabric.Thickness - 46 + 2 * 7.5F,
                Length = depth - 30,
                Thickness = corpusFabric.Thickness,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                Fabric = corpusFabric,
                FabricId = corpusFabric.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsRightBorder = false,
                IsLeftBorder = false
            });
            await _context.RectangularFormatters.AddAsync(new RectangularFormatter
            {
                Name = "FRONT SZUFLADY",
                Count = count,
                IsMilling = false,
                Width = width - 4,
                Length = frontHeight,
                Thickness = corpusFabric.Thickness,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                Fabric = frontFabric,
                FabricId = frontFabric.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                LeftBorderThickness = 1,
                RightBorderThickness = 1
            });

            await _context.SaveChangesAsync();
        }
    }
}