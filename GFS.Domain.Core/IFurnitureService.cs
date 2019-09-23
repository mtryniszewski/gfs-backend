using System.Threading.Tasks;
using GFS.Transfer.Furnitures.Commands;

namespace GFS.Domain.Core
{
    public interface IFurnitureService
    {
        Task<int> CreateSingleFormatterAsync(CreateSingleFormatterCommand command);
        Task<int> CreateBasicBottomAsync(CreateBasicBottomCommand command);
        Task<int> CreateOnlyDrawersBottomAsync(CreateOnlyDrawersBottomCommand command);
        Task<int> CreateThreePartsHighAsync(CreateThreePartsHighCommand command);
        Task<int> CreateSinkBottomAsync(CreateSinkBottomCommand command);
        Task<int> CreateTwoPartsHighAsync(CreateTwoPartsHighCommand command);
        Task<int> CreateBlindSideBottomAsync(CreateBlindSideBottomCommand command);
        Task<int> CreatePentagonCornerBottomAsync(CreatePentagonCornerBottom command);
        Task<int> CreateBasicWithDrawerBottomAsync(CreateBasicWithDrawerBottomCommand command);
        Task<int> CreateLCornerBottomAsync(CreateLCornerBottomCommand command);
        Task<int> CreateCutFinalBottomAsync(CreateCutFinalBottomCommand command);
        Task<int> CreateBasicTopAsync(CreateBasicTopCommand command);
        Task<int> CreateOneHorizontalTopAsync(CreateOneHorizontalTopCommand command);
        Task<int> CreateTwoHorizontalTopAsync(CreateTwoHorizontalTopCommand command);
        Task<int> CreateThreeHorizontalTopAsync(CreateThreeHorizontalTopCommand command);
        Task<int> CreateBasicGlassTopAsync(CreateBasicGlassTopCommand command);
        Task CreateOneHorizontalGlassTopAsync(CreateOneHorizontalGlassTopCommand command);
        Task CreateTwoHorizontalGlassTopAsync(CreateTwoHorizontalGlassTopCommand command);
        Task CreateThreeHorizontalGlassTopAsync(CreateThreeHorizontalGlassTopCommand command);
        Task DeleteFurnitureAsync(DeleteFurnitureCommand command);

    }
}