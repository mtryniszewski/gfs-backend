using System;
using System.Threading.Tasks;
using GFS.Core;
using GFS.Core.Enums;
using GFS.Data.EFCore;
using GFS.Data.Model.Entities;
using GFS.Domain.Core;
using GFS.Transfer.Furnitures.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GFS.Domain.Impl
{
    public class FurnitureService : IFurnitureService
    {
        private readonly GfsDbContext _context;
        private readonly Dictionary _dictionary;
        private readonly IDrawerService _drawerService;

        public FurnitureService(GfsDbContext context, IDrawerService drawerService, IOptions<Dictionary> dictionary)
        {
            _context = context;
            _drawerService = drawerService;
            _dictionary = dictionary.Value;
        }

        public async Task DeleteFurnitureAsync(DeleteFurnitureCommand command)
        {
            var furniture = await _context.Furnitures.FirstOrDefaultAsync(x => x.Id == command.Id);

            if (furniture == null)
                throw new GfsException(ErrorCode.FurnitureNotFound);

            _context.Furnitures.Remove(furniture);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateSingleFormatterAsync(CreateSingleFormatterCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.SingleFormatter);

            var order = await FindOrderAsync(command.OrderId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.SingleFormatter,
                OrderId = order.Id,
                Order = order,
                Name = command.Name,
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            var form = new RectangularFormatter
            {
                Name = "FORMATKA",
                Count = 1,
                IsMilling = false,
                Length = command.Length,
                Width = command.Width,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };

            await _context.RectangularFormatters.AddAsync(form);
            await _context.SaveChangesAsync();

            return furniture.Id;
        }

        #region  HighFurnitures

        //checked
        public async Task<int> CreateThreePartsHighAsync(CreateThreePartsHighCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.ThreePartsHigh);
            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var backFabric = await FindFabricAsync(command.BackFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.ThreePartsHigh,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            await CreateBottomCorpusAsync(command.Height, command.Depth, command.Width, corpusFabric, backFabric,
                furniture, false, true, true);

            var frontTop = new RectangularFormatter
            {
                Name = "FRONT GÓRNY",
                Count = 1,
                IsMilling = false,
                Length = command.TopFrontHeight,
                Width = command.Width - 4,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };

            var frontMiddle = new RectangularFormatter
            {
                Name = "FRONT ŚRODKOWY",
                Count = 1,
                IsMilling = false,
                Length = command.Height - command.TopFrontHeight - command.BottomFrontHeight - 4 - 4 - 4,
                Width = command.Width - 4,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };

            var frontBottom = new RectangularFormatter
            {
                Name = "FRONT DOLNY",
                Count = 1,
                IsMilling = false,
                Length = command.BottomFrontHeight,
                Width = command.Width - 4,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };

            var middleWreaths = new RectangularFormatter
            {
                Name = "FACHA",
                Count = 2,
                IsMilling = false,
                Length = command.Width - 2 * corpusFabric.Thickness,
                Width = command.Depth,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = true,
                IsRightBorder = false,
                LeftBorderThickness = 1
            };

            await _context.RectangularFormatters.AddAsync(frontTop);
            await _context.RectangularFormatters.AddAsync(frontMiddle);
            await _context.RectangularFormatters.AddAsync(frontBottom);
            await _context.RectangularFormatters.AddAsync(middleWreaths);

            if (command.ShelfCount > 0)
            {
                var shelves = new RectangularFormatter
                {
                    Name = "PÓŁKA",
                    Count = command.ShelfCount,
                    IsMilling = false,
                    Length = command.Width - 2 * corpusFabric.Thickness,
                    Width = command.Depth - 20,
                    Thickness = corpusFabric.Thickness,
                    FabricId = corpusFabric.Id,
                    Fabric = corpusFabric,
                    Furniture = furniture,
                    FurnitureId = furniture.Id,
                    IsTopBorder = false,
                    IsBottomBorder = false,
                    IsLeftBorder = true,
                    IsRightBorder = false,
                    LeftBorderThickness = 1
                };

                await _context.RectangularFormatters.AddAsync(shelves);
            }

            await _context.SaveChangesAsync();
            return furniture.Id;
        }

        //checked
        public async Task<int> CreateTwoPartsHighAsync(CreateTwoPartsHighCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.TwoPartsHigh);
            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var backFabric = await FindFabricAsync(command.BackFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.TwoPartsHigh,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            await CreateBottomCorpusAsync(command.Height, command.Depth, command.Width, corpusFabric, backFabric,
                furniture, false, true, true);

            var frontTop = new RectangularFormatter
            {
                Name = "FRONT GÓRNY",
                Count = 1,
                IsMilling = false,
                Length = command.Height - command.BottomFrontHeight - 4 - 4,
                Width = command.Width - 4,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };

            var frontBottom = new RectangularFormatter
            {
                Name = "FRONT DOLNY",
                Count = 1,
                IsMilling = false,
                Length = command.BottomFrontHeight,
                Width = command.Width - 4,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };

            var middleWreaths = new RectangularFormatter
            {
                Name = "FACHA",
                Count = 1,
                IsMilling = false,
                Length = command.Width - 2 * corpusFabric.Thickness,
                Width = command.Depth,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = true,
                IsRightBorder = false,
                LeftBorderThickness = 1
            };
            
            await _context.RectangularFormatters.AddAsync(frontTop);
            await _context.RectangularFormatters.AddAsync(frontBottom);
            await _context.RectangularFormatters.AddAsync(middleWreaths);

            if (command.ShelfCount > 0)
            {
                var shelves = new RectangularFormatter
                {
                    Name = "PÓŁKA",
                    Count = command.ShelfCount,
                    IsMilling = false,
                    Length = command.Width - 2 * corpusFabric.Thickness,
                    Width = command.Depth - 20,
                    Thickness = corpusFabric.Thickness,
                    FabricId = corpusFabric.Id,
                    Fabric = corpusFabric,
                    Furniture = furniture,
                    FurnitureId = furniture.Id,
                    IsTopBorder = false,
                    IsBottomBorder = false,
                    IsLeftBorder = true,
                    IsRightBorder = false,
                    LeftBorderThickness = 1
                };

                await _context.RectangularFormatters.AddAsync(shelves);
            }

            await _context.SaveChangesAsync();
            return furniture.Id;
        }
        #endregion

        #region BottomFurnitures

        //checked
        public async Task<int> CreateBlindSideBottomAsync(CreateBlindSideBottomCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.BlindSideBottom);
            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var backFabric = await FindFabricAsync(command.BackFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.BlindSideBottom,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            await CreateBottomCorpusAsync(command.Height, command.Depth, command.Width, corpusFabric, backFabric,
                furniture, true, false, true);

            var front = new RectangularFormatter
            {
                Name = "FRONT",
                Count = 1,
                IsMilling = false,
                Length = command.Height - 4,
                Width = command.FrontWidth,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };

            var blindSide = new RectangularFormatter
            {
                Name = "ŚLEPY BOK",
                Count = 1,
                IsMilling = false,
                Length = command.Height,
                Width = command.Width - command.FrontWidth - 50 - 18 - 4 - 2,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = true,
                RightBorderThickness = 1
            };

            var bar = new RectangularFormatter
            {
                Name = "LISTWA ZAŚLEPIAJĄCA",
                Count = 1,
                IsMilling = false,
                Length = command.Height,
                Width = 50,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = false,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                LeftBorderThickness = 1
            };

            await _context.RectangularFormatters.AddAsync(front);
            await _context.RectangularFormatters.AddAsync(blindSide);
            await _context.RectangularFormatters.AddAsync(bar);
            await _context.SaveChangesAsync();
            return furniture.Id;
        }

        //checked
        public async Task<int> CreateBasicBottomAsync(CreateBasicBottomCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.BasicBottom);
            CheckFrontCount(command.FrontCount);

            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var backFabric = await FindFabricAsync(command.BackFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.BasicBottom,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            await CreateBottomCorpusAsync(command.Height, command.Depth, command.Width, corpusFabric, backFabric,
                furniture, true, false, true);

            var front = new RectangularFormatter
            {
                Name = "FRONT",
                Count = command.FrontCount,
                IsMilling = false,
                Length = command.Height - 4,
                Width = (float)Math.Round((decimal)((command.Width - 4 - (command.FrontCount - 1) * 4) / command.FrontCount),MidpointRounding.AwayFromZero),
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };

            if (command.ShelfCount > 0)
            {
                var shelves = new RectangularFormatter
                {
                    Name = "PÓŁKA",
                    Count = command.ShelfCount,
                    IsMilling = false,
                    Length = command.Width - 2 * corpusFabric.Thickness,
                    Width = command.Depth - 20,
                    Thickness = corpusFabric.Thickness,
                    FabricId = corpusFabric.Id,
                    Fabric = corpusFabric,
                    Furniture = furniture,
                    FurnitureId = furniture.Id,
                    IsTopBorder = false,
                    IsBottomBorder = false,
                    IsLeftBorder = true,
                    IsRightBorder = false,
                    LeftBorderThickness = 1
                };
                await _context.RectangularFormatters.AddAsync(shelves);
            }


            await _context.RectangularFormatters.AddAsync(front);

            await _context.SaveChangesAsync();
            return furniture.Id;
        }

        //checked
        public async Task<int> CreateSinkBottomAsync(CreateSinkBottomCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.SinkBottom);
            CheckFrontCount(command.FrontCount);
            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var backFabric = await FindFabricAsync(command.BackFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.SinkBottom,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            await CreateBottomCorpusAsync(command.Height, command.Depth, command.Width, corpusFabric, backFabric,
                furniture, false, false, false);

            var front = new RectangularFormatter
            {
                Name = "FRONT",
                Count = command.FrontCount,
                IsMilling = false,
                Length = command.Height - 4,
                Width = (float)Math.Round((decimal)((command.Width - 4 - (command.FrontCount - 1) * 4) / command.FrontCount),MidpointRounding.AwayFromZero),
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };
            var backCrossbars = new RectangularFormatter
            {
                Name = "LISTWA",
                Count = 2,
                IsMilling = false,
                Length = command.Width - 2 * corpusFabric.Thickness,
                Width = 120,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = true,
                RightBorderThickness = 1
            };

            await _context.RectangularFormatters.AddAsync(front);
            await _context.RectangularFormatters.AddAsync(backCrossbars);
            await _context.SaveChangesAsync();

            return furniture.Id;
        }

        //checked
        public async Task<int> CreateCutFinalBottomAsync(CreateCutFinalBottomCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.CutFinalBottom);
            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var backFabric = await FindFabricAsync(command.BackFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.CutFinalBottom,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            var side1 = new RectangularFormatter
            {
                Name = "BOK PRAWY",
                Count = 1,
                IsMilling = false,
                Length = command.Height,
                Width = command.Depth1,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = true,
                IsRightBorder = false,
                LeftBorderThickness = 1
            };

            var side2 = new RectangularFormatter
            {
                Name = "BOK LEWY",
                Count = 1,
                IsMilling = false,
                Length = command.Height,
                Width = command.Depth2,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = true,
                RightBorderThickness = 1
            };

            var front = new RectangularFormatter
            {
                Name = "FRONT",
                Count = 1,
                IsMilling = false,
                Length = command.Height - 4,
                Width = (float)Math.Round((decimal)Math.Sqrt(Math.Pow(command.Depth1 - command.Depth2, 2) +
                                          Math.Pow(command.Width - 2 * corpusFabric.Thickness, 2)),MidpointRounding.AwayFromZero),
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };

            var bottomWreath = new PentagonFormatter
            {
                Name = "WIENIEC DOLNY",
                Count = 1,
                IsMilling = false,
                Width1 = (float)Math.Round((decimal)(Math.Sqrt(Math.Pow(command.Depth1 - command.Depth2, 2) +
                                           Math.Pow(command.Width - 2 * corpusFabric.Thickness, 2))),MidpointRounding.AwayFromZero),
                Width2 = command.Width - 2 * corpusFabric.Thickness,
                Depth1 = command.Depth1,
                Depth2 = command.Depth2,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = true,
                IsRightBorder = false,
                LeftBorderThickness = 1
            };

            var topWreath = new PentagonFormatter
            {
                Name = "WIENIEC GÓRNY",
                Count = 1,
                IsMilling = false,
                Width1 = (float) Math.Sqrt(Math.Pow(command.Depth1 - command.Depth2, 2) +
                                           Math.Pow(command.Width - 2 * corpusFabric.Thickness, 2)),
                Width2 = command.Width - 2 * corpusFabric.Thickness,
                Depth1 = command.Depth1,
                Depth2 = command.Depth2,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = true,
                RightBorderThickness = 1
            };

            if (command.ShelfCount > 0)
            {
                var shelves = new PentagonFormatter
                {
                    Name = "PÓŁKA",
                    Count = command.ShelfCount,
                    IsMilling = false,
                    Width1 = (float) Math.Sqrt(Math.Pow(command.Depth1 - command.Depth2, 2) +
                                               Math.Pow(command.Width - 2 * corpusFabric.Thickness, 2)),
                    Width2 = command.Width - 2 * corpusFabric.Thickness,
                    Depth1 = command.Depth1 - 20,
                    Depth2 = command.Depth1 - 20,
                    Thickness = corpusFabric.Thickness,
                    FabricId = corpusFabric.Id,
                    Fabric = corpusFabric,
                    Furniture = furniture,
                    FurnitureId = furniture.Id,
                    IsTopBorder = false,
                    IsBottomBorder = false,
                    IsLeftBorder = true,
                    IsRightBorder = false,
                    LeftBorderThickness = 1
                };
                await _context.PentagonFormatters.AddAsync(shelves);
                await _context.SaveChangesAsync();
            }

            var back = new RectangularFormatter
            {
                Name = "PLECY",
                Count = 1,
                IsMilling = false,
                Length = command.Height,
                Width = command.Width,
                Thickness = backFabric.Thickness,
                FabricId = backFabric.Id,
                Fabric = backFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = false
            };

            await _context.RectangularFormatters.AddAsync(back);
            await _context.RectangularFormatters.AddAsync(side1);
            await _context.RectangularFormatters.AddAsync(side2);
            await _context.RectangularFormatters.AddAsync(front);
            await _context.PentagonFormatters.AddAsync(bottomWreath);
            await _context.PentagonFormatters.AddAsync(topWreath);
            await _context.SaveChangesAsync();
            return furniture.Id;
        }

        //checked
        public async Task<int> CreateLCornerBottomAsync(CreateLCornerBottomCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.LCornerBottom);
            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var backFabric = await FindFabricAsync(command.BackFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.LCornerBottom,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            var front1 = new RectangularFormatter
            {
                Name = "FRONT PRAWY",
                Count = 1,
                IsMilling = false,
                Length = command.Height - 4,
                Width = command.Width1 - command.Depth - corpusFabric.Thickness - 2 - 5,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };

            var front2 = new RectangularFormatter
            {
                Name = "FRONT LEWY",
                Count = 1,
                IsMilling = false,
                Length = command.Height - 4,
                Width = command.Width2 - command.Depth - corpusFabric.Thickness - 2 - 2 - 5,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };

            var side1 = new RectangularFormatter
            {
                Name = "BOK PRAWY",
                Count = 1,
                IsMilling = false,
                Length = command.Height,
                Width = command.Depth,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = true,
                RightBorderThickness = 1
            };

            var side2 = new RectangularFormatter
            {
                Name = "BOK LEWY",
                Count = 1,
                IsMilling = false,
                Length = command.Height,
                Width = command.Depth,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = true,
                IsRightBorder = false,
                LeftBorderThickness = 1
            };

            var back = new RectangularFormatter
            {
                Name = "PLECY",
                Count = 1,
                IsMilling = false,
                Length = command.Width2 - corpusFabric.Thickness,
                Width = command.Height - 2 * corpusFabric.Thickness,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = true,
                RightBorderThickness = 1
            };

            var back2 = new RectangularFormatter
            {
                Name = "PLECY 2",
                Count = 1,
                IsMilling = false,
                Length = command.Width1 - corpusFabric.Thickness,
                Width = command.Height - 2 * corpusFabric.Thickness,
                Thickness = backFabric.Thickness,
                FabricId = backFabric.Id,
                Fabric = backFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = false
            };

            var wreaths = new LFormatter
            {
                Name = "WIENIEC DOLNY I GÓRNY",
                Count = 2,
                IsMilling = false,
                Width1 = command.Width1 - corpusFabric.Thickness,
                Width2 = command.Width2 - corpusFabric.Thickness,
                Depth1 = command.Depth,
                Depth2 = command.Depth,
                Indentation1 = command.Width1 - corpusFabric.Thickness - command.Depth,
                Indentation2 = command.Width2 - corpusFabric.Thickness - command.Depth,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsDepth1Border = false,
                IsDepth2Border = false,
                IsIndentation1Border = true,
                IsIndentation2Border = true,
                IsWidth1Border = false,
                IsWidth2Border = false,
                Indentation1BorderThickness = 1,
                Indentation2BorderThickness = 1
            };

            await _context.RectangularFormatters.AddAsync(front1);
            await _context.RectangularFormatters.AddAsync(front2);
            await _context.RectangularFormatters.AddAsync(side1);
            await _context.RectangularFormatters.AddAsync(side2);
            await _context.RectangularFormatters.AddAsync(back);
            await _context.RectangularFormatters.AddAsync(back2);
            await _context.LFormatters.AddAsync(wreaths);
            await _context.SaveChangesAsync();


            if (command.ShelfCount > 0)
            {
                var shelves = new LFormatter
                {
                    Name = "PÓŁKA",
                    Count = command.ShelfCount,
                    IsMilling = false,
                    Width1 = command.Width1 - corpusFabric.Thickness,
                    Width2 = command.Width2 - corpusFabric.Thickness,
                    Depth1 = command.Depth - 20,
                    Depth2 = command.Depth - 20,
                    Indentation1 = command.Width1 - corpusFabric.Thickness - command.Depth + 20,
                    Indentation2 = command.Width2 - corpusFabric.Thickness - command.Depth + 20,
                    Thickness = corpusFabric.Thickness,
                    FabricId = corpusFabric.Id,
                    Fabric = corpusFabric,
                    Furniture = furniture,
                    FurnitureId = furniture.Id,
                    IsDepth1Border = false,
                    IsDepth2Border = false,
                    IsIndentation1Border = true,
                    IsIndentation2Border = true,
                    IsWidth1Border = false,
                    IsWidth2Border = false,
                    Indentation1BorderThickness = 1,
                    Indentation2BorderThickness = 1
                };

                await _context.LFormatters.AddAsync(shelves);
                await _context.SaveChangesAsync();
            }

            return furniture.Id;
        }

        //checked
        public async Task<int> CreateOnlyDrawersBottomAsync(CreateOnlyDrawersBottomCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.OnlyDrawersBottom);
            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);
            var backFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.OnlyDrawersBottom,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.AddAsync(furniture);
            await _context.SaveChangesAsync();

            await CreateBottomCorpusAsync(command.Height, command.Depth, command.Width, corpusFabric, backFabric,
                furniture, true, false, true);

            if (command.DrawerConfiguration == DrawerConfiguration.TwoHigh)
                await _drawerService.CreateOnlyHighAsync(command.DrawerType, command.Height, command.Width,
                    command.Depth,
                    corpusFabric, frontFabric, furniture, 2);
            else if (command.DrawerConfiguration == DrawerConfiguration.FiveShort)
                await _drawerService.CreateOnlyShortAsync(command.DrawerType, command.Height, command.Width,
                    command.Depth,
                    corpusFabric, frontFabric, furniture, 5);
            else if (command.DrawerConfiguration == DrawerConfiguration.TwoHighOneShort)
                await _drawerService.CreateMixedConfigAsync(command.DrawerType, command.Height, command.Width,
                    command.Depth,
                    corpusFabric, frontFabric, furniture, 1, 2);
            else if (command.DrawerConfiguration == DrawerConfiguration.OneHighThreeShort)
                await _drawerService.CreateMixedConfigAsync(command.DrawerType, command.Height, command.Width,
                    command.Depth,
                    corpusFabric, frontFabric, furniture, 3, 1);
            else
                throw new GfsException(ErrorCode.WrongDrawerConfiguration, _dictionary.WrongDrawerConfiguration);

            return furniture.Id;
        }

        //checked
        public async Task<int> CreatePentagonCornerBottomAsync(CreatePentagonCornerBottom command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.PentagonCornerBottom);
            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var backFabric = await FindFabricAsync(command.BackFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.PentagonCornerBottom,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            var side1 = new RectangularFormatter
            {
                Name = "BOK PRAWY",
                Count = 1,
                IsMilling = false,
                Length = command.Height,
                Width = command.Depth1,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = true,
                IsRightBorder = false,
                LeftBorderThickness = 1
            };

            var side2 = new RectangularFormatter
            {
                Name = "BOK LEWY",
                Count = 1,
                IsMilling = false,
                Length = command.Height,
                Width = command.Depth2,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = true,
                RightBorderThickness = 1
            };

            var front = new RectangularFormatter
            {
                Name = "FRONT",
                Count = 1,
                IsMilling = false,
                Length = command.Height - 4,
                Width = (float)Math.Round((decimal)(Math.Sqrt(Math.Pow(command.Width1 - command.Depth1, 2) +
                                           Math.Pow(command.Width2 - command.Depth2, 2)) - 25),MidpointRounding.AwayFromZero),
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                LeftBorderThickness = 1,
                RightBorderThickness = 1,
                TopBorderThickness = 1,
                BottomBorderThickness = 1
            };

            var wreaths = new PentagonFormatter
            {
                Name = "WIENIEC DOLNY I GÓRNY",
                Count = 2,
                IsMilling = false,
                Width1 = command.Width1 - 2 * corpusFabric.Thickness,
                Width2 = command.Width2 - 2 * corpusFabric.Thickness,
                Depth1 = command.Depth1 - corpusFabric.Thickness,
                Depth2 = command.Depth2 - corpusFabric.Thickness,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = false,
                IsHypotenuseBorder = true,
                HypotenuseBorderThickness = 1
            };

            var back1 = new RectangularFormatter
            {
                Name = "PLECY PRAWE",
                Count = 1,
                IsMilling = false,
                Length = command.Height,
                Width = command.Width1 - 2 * corpusFabric.Thickness,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = false
            };

            var back2 = new RectangularFormatter
            {
                Name = "PLECY LEWE",
                Count = 1,
                IsMilling = false,
                Length = command.Height,
                Width = command.Width2 - corpusFabric.Thickness,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = false
            };

            if (command.ShelfCount > 0)
            {
                var shelves = new PentagonFormatter
                {
                    Name = "PÓŁKA",
                    Count = command.ShelfCount,
                    IsMilling = false,
                    Width1 = command.Width1 - 2 * corpusFabric.Thickness,
                    Width2 = command.Width2 - 2 * corpusFabric.Thickness,
                    Depth1 = command.Depth1 - corpusFabric.Thickness - 20,
                    Depth2 = command.Depth2 - corpusFabric.Thickness - 20,
                    Thickness = corpusFabric.Thickness,
                    FabricId = corpusFabric.Id,
                    Fabric = corpusFabric,
                    Furniture = furniture,
                    FurnitureId = furniture.Id,
                    IsTopBorder = false,
                    IsBottomBorder = false,
                    IsLeftBorder = false,
                    IsRightBorder = false,
                    IsHypotenuseBorder = true,
                    HypotenuseBorderThickness = 1
                };
                await _context.PentagonFormatters.AddAsync(shelves);
            }


            await _context.RectangularFormatters.AddAsync(side1);
            await _context.RectangularFormatters.AddAsync(side2);
            await _context.RectangularFormatters.AddAsync(front);
            await _context.PentagonFormatters.AddAsync(wreaths);
            await _context.RectangularFormatters.AddAsync(back1);
            await _context.RectangularFormatters.AddAsync(back2);
            await _context.SaveChangesAsync();
            return furniture.Id;
        }

        //checked
        public async Task<int> CreateBasicWithDrawerBottomAsync(CreateBasicWithDrawerBottomCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.BasicWithDrawerBottom);
            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var backFabric = await FindFabricAsync(command.BackFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.BasicWithDrawerBottom,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            await CreateBottomCorpusAsync(command.Height, command.Depth, command.Width, corpusFabric, backFabric,
                furniture, true, false, true);

            var front = new RectangularFormatter
            {
                Name = "FRONT",
                Count = command.FrontCount,
                IsMilling = false,
                Length = command.Height - 140 - 4,
                Width = (float)Math.Round((decimal)((command.Width - (command.FrontCount + 1) * 4) / command.FrontCount),MidpointRounding.AwayFromZero),
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };
            var middleWreath = new RectangularFormatter
            {
                Name = "FACHA",
                Count = 1,
                IsMilling = false,
                Length = command.Width - 2 * corpusFabric.Thickness,
                Width = command.Depth,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = true,
                IsRightBorder = false,
                LeftBorderThickness = 1
            };
            var shelves = new RectangularFormatter
            {
                Name = "PÓŁKA",
                Count = command.ShelfCount,
                IsMilling = false,
                Length = command.Width - 2 * corpusFabric.Thickness,
                Width = command.Depth - 20,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = true,
                IsRightBorder = false,
                LeftBorderThickness = 1
            };


            await _context.RectangularFormatters.AddAsync(front);
            await _context.RectangularFormatters.AddAsync(middleWreath);
            await _context.RectangularFormatters.AddAsync(shelves);

            if (command.DrawerType == DrawerType.AmixBlum)
                await _drawerService.CreateAmixBlumDrawersAsync(command.Width, command.Depth, 140, 90, corpusFabric,
                    frontFabric, 1, furniture);
            if (command.DrawerType == DrawerType.Versalite)
                await _drawerService.CreateVersaliteDrawersAsync(command.Width, command.Depth, 140, 90, corpusFabric,
                    frontFabric, 1, furniture);

            await _context.SaveChangesAsync();
            return furniture.Id;
        }

        #endregion

        #region TopFurnitures

        //checked
        public async Task<int> CreateBasicTopAsync(CreateBasicTopCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.BasicTop);
            CheckFrontCount(command.FrontCount);
            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var backFabric = await FindFabricAsync(command.BackFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.BasicTop,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            await CreateTopCorpusAsync(command.Height, command.Depth, command.Width, corpusFabric, backFabric,
                furniture, false, GetMiddleWreathCount(command.FurnitureType));

            var front = new RectangularFormatter
            {
                Name = "FRONT",
                Count = command.FrontCount,
                IsMilling = false,
                Length = command.Height - 4,
                Width = (float)Math.Round((decimal)((command.Width - 4 - (command.FrontCount - 1) * 4) / command.FrontCount),MidpointRounding.AwayFromZero),
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };

            if (command.ShelfCount > 0)
            {
                var shelves = new RectangularFormatter
                {
                    Name = "PÓŁKA",
                    Count = command.ShelfCount,
                    IsMilling = false,
                    Length = command.Width - 2 * corpusFabric.Thickness,
                    Width = command.Depth - 40,
                    Thickness = corpusFabric.Thickness,
                    FabricId = corpusFabric.Id,
                    Fabric = corpusFabric,
                    Furniture = furniture,
                    FurnitureId = furniture.Id,
                    IsTopBorder = false,
                    IsBottomBorder = false,
                    IsLeftBorder = true,
                    IsRightBorder = false,
                    LeftBorderThickness = 1
                };
                await _context.RectangularFormatters.AddAsync(shelves);
                await _context.SaveChangesAsync();
            }


            await _context.RectangularFormatters.AddAsync(front);
            await _context.SaveChangesAsync();

            return furniture.Id;
        }

        //checked
        public async Task<int> CreateBasicGlassTopAsync(CreateBasicGlassTopCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.BasicGlassTop);
            CheckFrontCount(command.FrontCount);

            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var backFabric = await FindFabricAsync(command.BackFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.BasicGlassTop,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            await CreateTopCorpusAsync(command.Height, command.Depth, command.Width, corpusFabric, backFabric,
                furniture, true, GetMiddleWreathCount(command.FurnitureType));

            var leftCrossbars = new RectangularFormatter
            {
                Name = "LISTWA LEWA",
                Count = command.FrontCount,
                IsMilling = false,
                Length = command.Height - 2,
                Width = command.FrameThickness,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                LeftBorderThickness = 1,
                RightBorderThickness = 1
            };

            var rightCrossbars = new RectangularFormatter
            {
                Name = "LISTWA PRAWA",
                Count = command.FrontCount,
                IsMilling = false,
                Length = command.Height - 2,
                Width = command.FrameThickness,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                LeftBorderThickness = 1,
                RightBorderThickness = 1
            };

            var topCrossbars = new RectangularFormatter
            {
                Name = "LISTWA GÓRA",
                Count = command.FrontCount,
                IsMilling = false,
                Length = (command.Width - 4 * command.FrontCount - 2 * command.FrontCount * command.FrameThickness) /
                         command.FrontCount,
                Width = command.FrameThickness,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                LeftBorderThickness = 1,
                RightBorderThickness = 1
            };

            var bottomCrossbars = new RectangularFormatter
            {
                Name = "LISTWA DÓŁ",
                Count = command.FrontCount,
                IsMilling = false,
                Length = (command.Width - 4 * command.FrontCount - 2 * command.FrontCount * command.FrameThickness) /
                         command.FrontCount,
                Width = command.FrameThickness,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                LeftBorderThickness = 1,
                RightBorderThickness = 1
            };

            var glass = new RectangularFormatter
            {
                Name = "SZKŁO FRONT",
                Count = command.FrontCount,
                IsMilling = false,
                Length = (command.Width - 4 * command.FrontCount - 2 * command.FrontCount * command.FrameThickness) /
                         command.FrontCount + 10,
                Width = command.FrameThickness,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = false
            };


            await _context.RectangularFormatters.AddAsync(leftCrossbars);
            await _context.RectangularFormatters.AddAsync(rightCrossbars);
            await _context.RectangularFormatters.AddAsync(topCrossbars);
            await _context.RectangularFormatters.AddAsync(bottomCrossbars);
            await _context.RectangularFormatters.AddAsync(glass);
            await _context.SaveChangesAsync();

            return furniture.Id;
        }

        //checked
        public async Task<int> CreateOneHorizontalTopAsync(CreateOneHorizontalTopCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.OneHorizontalTop);
            CheckFrontCount(command.FrontCount);

            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var backFabric = await FindFabricAsync(command.BackFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.OneHorizontalTop,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            await CreateTopCorpusAsync(command.Height, command.Depth, command.Width, corpusFabric, backFabric,
                furniture, false, GetMiddleWreathCount(command.FurnitureType));


            var front = new RectangularFormatter
            {
                Name = "FRONT",
                Count = 1,
                IsMilling = false,
                Length = command.Width - 4,
                Width = (float)Math.Round((decimal)((command.Height - command.FrontCount * 4) / command.FrontCount),MidpointRounding.AwayFromZero),
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1,
                LeftBorderThickness = 1
            };


            await _context.RectangularFormatters.AddAsync(front);
            await _context.SaveChangesAsync();


            if (command.ShelfCount > 0)
            {
                var shelves = new RectangularFormatter
                {
                    Name = "PÓŁKA",
                    Count = command.ShelfCount,
                    IsMilling = false,
                    Length = command.Width - 2 * corpusFabric.Thickness,
                    Width = command.Depth - 40,
                    Thickness = corpusFabric.Thickness,
                    FabricId = corpusFabric.Id,
                    Fabric = corpusFabric,
                    Furniture = furniture,
                    FurnitureId = furniture.Id,
                    IsTopBorder = false,
                    IsBottomBorder = false,
                    IsLeftBorder = true,
                    IsRightBorder = false,
                    LeftBorderThickness = 1
                };

                await _context.RectangularFormatters.AddAsync(shelves);
                await _context.SaveChangesAsync();
            }

            return furniture.Id;
        }

        //checked
        public async Task<int> CreateTwoHorizontalTopAsync(CreateTwoHorizontalTopCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.TwoHorizontalTop);
            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var backFabric = await FindFabricAsync(command.BackFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.TwoHorizontalTop,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            await CreateTopCorpusAsync(command.Height, command.Depth, command.Width, corpusFabric, backFabric,
                furniture, true, GetMiddleWreathCount(command.FurnitureType));

            var fronts = new RectangularFormatter
            {
                Name = "FRONT",
                Count = 2,
                IsMilling = false,
                Length = command.Width - 4,
                Width = (float)Math.Round((decimal)((command.Height - 2 * 4) / 2),MidpointRounding.AwayFromZero),
                Thickness = frontFabric.Thickness,
                Fabric = frontFabric,
                FabricId = frontFabric.Id,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                IsTopBorder = true,
                LeftBorderThickness = 1,
                RightBorderThickness = 1,
                TopBorderThickness = 1,
                BottomBorderThickness = 1
            };
            await _context.RectangularFormatters.AddAsync(fronts);
            await _context.SaveChangesAsync();

            if (command.ShelfCount > 0)
            {
                var shelves = new RectangularFormatter
                {
                    Name = "PÓŁKA",
                    Count = command.ShelfCount,
                    IsMilling = false,
                    Length = command.Width - 2 * corpusFabric.Thickness,
                    Width = command.Depth - 40,
                    Thickness = corpusFabric.Thickness,
                    FabricId = corpusFabric.Id,
                    Fabric = corpusFabric,
                    Furniture = furniture,
                    FurnitureId = furniture.Id,
                    IsTopBorder = false,
                    IsBottomBorder = false,
                    IsLeftBorder = true,
                    IsRightBorder = false,
                    LeftBorderThickness = 1
                };

                await _context.RectangularFormatters.AddAsync(shelves);
                await _context.SaveChangesAsync();
            }


            await _context.SaveChangesAsync();

            return furniture.Id;
        }

        //checked
        public async Task<int> CreateThreeHorizontalTopAsync(CreateThreeHorizontalTopCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.ThreeHorizontalTop);


            var order = await FindOrderAsync(command.OrderId);
            var frontFabric = await FindFabricAsync(command.FrontFabricId);
            var backFabric = await FindFabricAsync(command.BackFabricId);
            var corpusFabric = await FindFabricAsync(command.CorpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.ThreeHorizontalTop,
                OrderId = order.Id,
                Order = order,
                Name = command.Name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            await CreateTopCorpusAsync(command.Height, command.Depth, command.Width, corpusFabric, backFabric,
                furniture, true, GetMiddleWreathCount(command.FurnitureType));

            var fronts = new RectangularFormatter
            {
                Name = "FRONT",
                Count = 3,
                IsMilling = false,
                Length = command.Width - 4,
                Width = (float)Math.Round((decimal)((command.Height - 3 * 4) / 3),MidpointRounding.AwayFromZero),
                Thickness = frontFabric.Thickness,
                Fabric = frontFabric,
                FabricId = frontFabric.Id,
                FurnitureId = furniture.Id,
                Furniture = furniture,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                LeftBorderThickness = 1,
                RightBorderThickness = 1,
                TopBorderThickness = 1,
                BottomBorderThickness = 1
            };

            await _context.RectangularFormatters.AddAsync(fronts);
            await _context.SaveChangesAsync();

            if (command.ShelfCount > 0)
            {
                var shelves = new RectangularFormatter
                {
                    Name = "PÓŁKA",
                    Count = command.ShelfCount,
                    IsMilling = false,
                    Length = command.Width - 2 * corpusFabric.Thickness,
                    Width = command.Depth - 40,
                    Thickness = corpusFabric.Thickness,
                    FabricId = corpusFabric.Id,
                    Fabric = corpusFabric,
                    Furniture = furniture,
                    FurnitureId = furniture.Id,
                    IsTopBorder = false,
                    IsBottomBorder = false,
                    IsLeftBorder = true,
                    IsRightBorder = false,
                    LeftBorderThickness = 1
                };

                await _context.RectangularFormatters.AddAsync(shelves);
                await _context.SaveChangesAsync();
            }

            return furniture.Id;
        }

        //checked
        public async Task CreateOneHorizontalGlassTopAsync(CreateOneHorizontalGlassTopCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.OneHorizontalGlassTop);

            await CreateHorizontalGlassTopAsync(command.FurnitureType, command.FrameThickness, command.Name, 1,
                command.OrderId, command.FrontFabricId, command.BackFabricId, command.CorpusFabricId, command.Width,
                command.Depth, command.Height, command.ShelfCount);
        }

        //checked
        public async Task CreateTwoHorizontalGlassTopAsync(CreateTwoHorizontalGlassTopCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.TwoHorizontalGlassTop);

            await CreateHorizontalGlassTopAsync(command.FurnitureType, command.FrameThickness, command.Name, 2,
                command.OrderId, command.FrontFabricId, command.BackFabricId, command.CorpusFabricId, command.Width,
                command.Depth, command.Height, command.ShelfCount);
        }

        //checked
        public async Task CreateThreeHorizontalGlassTopAsync(CreateThreeHorizontalGlassTopCommand command)
        {
            CheckFurnitureType(command.FurnitureType, FurnitureType.ThreeHorizontalGlassTop);

            await CreateHorizontalGlassTopAsync(command.FurnitureType, command.FrameThickness, command.Name, 3,
                command.OrderId, command.FrontFabricId, command.BackFabricId, command.CorpusFabricId, command.Width,
                command.Depth, command.Height, command.ShelfCount);
        }

        #endregion



        #region PrivateBottomMethods

        //checked
        private async Task CreateBottomCorpusAsync(float height, float depth, float width, Fabric corpusFabric,
            Fabric backFabric, Furniture furniture, bool isCrossbars, bool isTopWreath, bool isBack)
        {
            if (isTopWreath && isCrossbars)
                throw new GfsException(ErrorCode.WrongFurnitureType, _dictionary.WrongFurnitureType);


            var rightSide = new RectangularFormatter
            {
                Name = "BOK PRAWY",
                Count = 1,
                IsMilling = false,
                Length = height,
                Width = depth,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = true,
                IsRightBorder = false,
                LeftBorderThickness = 1
            };

            await _context.RectangularFormatters.AddAsync(rightSide);
            var leftSide = new RectangularFormatter
            {
                Name = "BOK LEWY",
                Count = 1,
                IsMilling = false,
                Length = height,
                Width = depth,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = true,
                RightBorderThickness = 1
            };
            await _context.RectangularFormatters.AddAsync(leftSide);

            var bottomWreath = new RectangularFormatter
            {
                Name = "WIENIEC DOLNY",
                Count = 1,
                IsMilling = false,
                Length = width - 2 * corpusFabric.Thickness,
                Width = depth,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = true,
                IsRightBorder = false,
                LeftBorderThickness = 1
            };

            await _context.RectangularFormatters.AddAsync(bottomWreath);

            if (isCrossbars)
            {
                var crossbars = new RectangularFormatter
                {
                    Name = "TRAWERS",
                    Count = 2,
                    IsMilling = false,
                    Length = width - 2 * corpusFabric.Thickness,
                    Width = 80,
                    Thickness = corpusFabric.Thickness,
                    FabricId = corpusFabric.Id,
                    Fabric = corpusFabric,
                    Furniture = furniture,
                    FurnitureId = furniture.Id,
                    IsTopBorder = false,
                    IsBottomBorder = false,
                    IsLeftBorder = true,
                    IsRightBorder = false,
                    LeftBorderThickness = 1
                };
                await _context.RectangularFormatters.AddAsync(crossbars);
            }

            if (isTopWreath)
            {
                var topWreath = new RectangularFormatter
                {
                    Name = "WIENIEC GÓRNY",
                    Count = 1,
                    IsMilling = false,
                    Length = width - 2 * corpusFabric.Thickness,
                    Width = depth,
                    Thickness = corpusFabric.Thickness,
                    FabricId = corpusFabric.Id,
                    Fabric = corpusFabric,
                    Furniture = furniture,
                    FurnitureId = furniture.Id,
                    IsTopBorder = false,
                    IsBottomBorder = false,
                    IsLeftBorder = true,
                    IsRightBorder = false,
                    LeftBorderThickness = 1
                };
                await _context.RectangularFormatters.AddAsync(topWreath);
            }

            if (isBack)
            {
                var back = new RectangularFormatter
                {
                    Name = "PLECY",
                    Count = 1,
                    IsMilling = false,
                    Length = height,
                    Width = width,
                    Thickness = backFabric.Thickness,
                    FabricId = backFabric.Id,
                    Fabric = backFabric,
                    Furniture = furniture,
                    FurnitureId = furniture.Id,
                    IsTopBorder = false,
                    IsBottomBorder = false,
                    IsLeftBorder = false,
                    IsRightBorder = false
                };
                await _context.RectangularFormatters.AddAsync(back);
            }


            await _context.SaveChangesAsync();
        }
        #endregion

        #region PrivateCommonMethods

        private async Task<Order> FindOrderAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null)
                throw new GfsException(ErrorCode.OrderNotFound, _dictionary.OrderNotFound);

            return order;
        }

        private async Task<Fabric> FindFabricAsync(int fabricId)
        {
            var fabric = await _context.Fabrics.FirstOrDefaultAsync(x => x.Id == fabricId);
            if (fabric == null)
                throw new GfsException(ErrorCode.FabricNotFound, _dictionary.FabricNotFound);
            return fabric;
        }

        private void CheckFurnitureType(FurnitureType fromCommand, FurnitureType fromEnum)
        {
            if (fromCommand != fromEnum)
                throw new GfsException(ErrorCode.WrongFurnitureType, _dictionary.WrongFurnitureType);
        }

        private void CheckFrontCount(int fromCommand)
        {
            if (fromCommand != 1 && fromCommand != 2)
                throw new GfsException(ErrorCode.WrongFrontCount, _dictionary.WrongFrontCount);
        }

        #endregion

        #region PrivateTopMethods

        //checked
        private async Task CreateTopCorpusAsync(float height, float depth, float width, Fabric corpusFabric,
            Fabric backFabric, Furniture furniture, bool isMiddleWreath, int middleWreathCount)
        {
            var rightSide = new RectangularFormatter
            {
                Name = "BOK PRAWY",
                Count = 1,
                IsMilling = true,
                Milling = Milling.LengthMilling,
                Length = height,
                Width = depth,
                CutterLength = height,
                CutterWidth = 4,
                CutterDepth = 11,
                LeftSpace = depth - 20,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = false,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                LeftBorderThickness = 1
            };

            await _context.RectangularFormatters.AddAsync(rightSide);

            var leftSide = new RectangularFormatter
            {
                Name = "BOK LEWY",
                Count = 1,
                IsMilling = true,
                Milling = Milling.LengthMilling,
                Length = height,
                Width = depth,
                CutterLength = height,
                CutterWidth = 4,
                CutterDepth = 11,
                LeftSpace = depth - 20,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = false,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1
            };

            await _context.RectangularFormatters.AddAsync(leftSide);

            var topWreath = new RectangularFormatter
            {
                Name = "WIENIEC GÓRNY",
                Count = 1,
                IsMilling = false,
                Length = width - 2 * corpusFabric.Thickness,
                Width = depth - 20,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = false,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                RightBorderThickness = 1
            };

            await _context.RectangularFormatters.AddAsync(topWreath);

            if (isMiddleWreath)
            {
                var middleWreath = new RectangularFormatter
                {
                    Name = "FACHA",
                    Count = middleWreathCount,
                    IsMilling = false,
                    Length = width - 2 * corpusFabric.Thickness,
                    Width = depth - 20,
                    Thickness = corpusFabric.Thickness,
                    FabricId = corpusFabric.Id,
                    Fabric = corpusFabric,
                    Furniture = furniture,
                    FurnitureId = furniture.Id,
                    IsTopBorder = true,
                    IsBottomBorder = true,
                    IsLeftBorder = false,
                    IsRightBorder = true,
                    TopBorderThickness = 1,
                    BottomBorderThickness = 1,
                    RightBorderThickness = 1
                };

                await _context.RectangularFormatters.AddAsync(middleWreath);
                await _context.SaveChangesAsync();
            }

            var bottomWreath = new RectangularFormatter
            {
                Name = "WIENIEC DOLNY",
                Count = 1,
                IsMilling = true,
                Milling = Milling.LengthMilling,
                Length = width - 2 * corpusFabric.Thickness,
                Width = depth,
                CutterLength = height,
                CutterWidth = 4,
                CutterDepth = 11,
                LeftSpace = depth - 20,
                Thickness = corpusFabric.Thickness,
                FabricId = corpusFabric.Id,
                Fabric = corpusFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = false,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                LeftBorderThickness = 1
            };

            await _context.RectangularFormatters.AddAsync(bottomWreath);

            var back = new RectangularFormatter
            {
                Name = "PLECY",
                Count = 1,
                IsMilling = false,
                Length = height,
                Width = width,
                Thickness = backFabric.Thickness,
                FabricId = backFabric.Id,
                Fabric = backFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = false,
                IsBottomBorder = false,
                IsLeftBorder = false,
                IsRightBorder = false
            };
            await _context.RectangularFormatters.AddAsync(back);

            await _context.SaveChangesAsync();
        }

        private async Task<int> CreateHorizontalGlassTopAsync(FurnitureType furnitureType, float frameThickness,
            string name, int frontCount, int orderId, int frontFabricId, int backFabricId, int corpusFabricId,
            float width, float depth, float height, int shelfCount)
        {
            CheckFrontCount(frontCount);

            var order = await FindOrderAsync(orderId);
            var frontFabric = await FindFabricAsync(frontFabricId);
            var backFabric = await FindFabricAsync(backFabricId);
            var corpusFabric = await FindFabricAsync(corpusFabricId);

            var furniture = new Furniture
            {
                FurnitureType = FurnitureType.TwoHorizontalGlassTop,
                OrderId = order.Id,
                Order = order,
                Name = name
            };

            await _context.Furnitures.AddAsync(furniture);
            await _context.SaveChangesAsync();

            await CreateTopCorpusAsync(height, depth, width, corpusFabric, backFabric,
                furniture, true, GetMiddleWreathCount(furnitureType));

            var leftCrossbars = new RectangularFormatter
            {
                Name = "LISTWA LEWA",
                Count = frontCount,
                IsMilling = false,
                Length = (height - frontCount * 4) / frontCount,
                Width = frameThickness,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                LeftBorderThickness = 1,
                RightBorderThickness = 1
            };

            var rightCrossbars = new RectangularFormatter
            {
                Name = "LISTWA PRAWA",
                Count = frontCount,
                IsMilling = false,
                Length = (height - frontCount * 4) / frontCount,
                Width = frameThickness,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                LeftBorderThickness = 1,
                RightBorderThickness = 1
            };

            var topCrossbars = new RectangularFormatter
            {
                Name = "LISTWA GÓRA",
                Count = frontCount,
                IsMilling = false,
                Length = width - 4 - 2 * frameThickness,
                Width = frameThickness,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                LeftBorderThickness = 1,
                RightBorderThickness = 1
            };

            var bottomCrossbars = new RectangularFormatter
            {
                Name = "LISTWA GÓRA",
                Count = frontCount,
                IsMilling = false,
                Length = width - 4 - 2 * frameThickness,
                Width = frameThickness,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id,
                IsTopBorder = true,
                IsBottomBorder = true,
                IsLeftBorder = true,
                IsRightBorder = true,
                TopBorderThickness = 1,
                BottomBorderThickness = 1,
                LeftBorderThickness = 1,
                RightBorderThickness = 1
            };

            var glass = new RectangularFormatter
            {
                Name = "SZKŁO FRONT",
                Count = frontCount,
                IsMilling = false,
                Length = width - 4 - 2 * frameThickness + 10,
                Width = (height - frontCount * 4) / frontCount + 10,
                Thickness = frontFabric.Thickness,
                FabricId = frontFabric.Id,
                Fabric = frontFabric,
                Furniture = furniture,
                FurnitureId = furniture.Id
            };

            if (shelfCount > 0)
            {
                var shelves = new RectangularFormatter
                {
                    Name = "PÓŁKA",
                    Count = shelfCount,
                    Width = depth - 40,
                    Length = width - 2 * corpusFabric.Thickness,
                    Thickness = corpusFabric.Thickness,
                    Fabric = corpusFabric,
                    FabricId = corpusFabric.Id,
                    FurnitureId = furniture.Id,
                    Furniture = furniture,
                    IsMilling = false,
                    IsBottomBorder = false,
                    IsLeftBorder = true,
                    IsRightBorder = false,
                    IsTopBorder = false,
                    LeftBorderThickness = 1
                };
            }

            await _context.RectangularFormatters.AddAsync(glass);
            await _context.RectangularFormatters.AddAsync(leftCrossbars);
            await _context.RectangularFormatters.AddAsync(rightCrossbars);
            await _context.RectangularFormatters.AddAsync(topCrossbars);
            await _context.RectangularFormatters.AddAsync(bottomCrossbars);
            await _context.SaveChangesAsync();

            return furniture.Id;
        }



        private int GetMiddleWreathCount(FurnitureType furnitureType)
        {
            switch (furnitureType)
            {
                case FurnitureType.BasicTop:
                    return 0;
                case FurnitureType.OneHorizontalTop:
                    return 0;
                case FurnitureType.TwoHorizontalTop:
                    return 1;
                case FurnitureType.ThreeHorizontalTop:
                    return 2;
                case FurnitureType.TwoHorizontalGlassTop:
                    return 1;
                case FurnitureType.BasicGlassTop:
                    return 0;
                default:
                    throw new GfsException(ErrorCode.WrongFurnitureType);
            }
        }

        #endregion
    }
}