using System.Threading.Tasks;
using GFS.Domain.Core;
using GFS.Transfer.Furnitures.Commands;
using GFS.Transfer.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GFS.Web.Controllers
{
    [Route("api/[controller]")]
    public class FurnitureController : Controller
    {
        private readonly IFurnitureService _furnitureService;

        public FurnitureController(IFurnitureService furnitureService)
        {
            _furnitureService = furnitureService;
        }

        [HttpPost]
        [Route("singleformatter")]
        public async Task<IActionResult> PostSingleFormatter([FromBody] CreateSingleFormatterCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateSingleFormatterAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("basicbottom")]
        public async Task<IActionResult> PostBasicBottom([FromBody] CreateBasicBottomCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateBasicBottomAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("onlydrawersbottom")]
        public async Task<IActionResult> PostOnlyDrawersBottom([FromBody] CreateOnlyDrawersBottomCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateOnlyDrawersBottomAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("threepartshigh")]
        public async Task<IActionResult> PostThreePartsHigh([FromBody] CreateThreePartsHighCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateThreePartsHighAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("twopartshigh")]
        public async Task<IActionResult> PostTwoPartsHigh([FromBody] CreateTwoPartsHighCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateTwoPartsHighAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("sinkbottom")]
        public async Task<IActionResult> PostSinkBottom([FromBody] CreateSinkBottomCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateSinkBottomAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("blindsidebottom")]
        public async Task<IActionResult> PostBlindSideBottom([FromBody] CreateBlindSideBottomCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateBlindSideBottomAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("pentagoncornerbottom")]
        public async Task<IActionResult> PostPentagonCornerBottom([FromBody] CreatePentagonCornerBottom command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreatePentagonCornerBottomAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("basicwithdrawerbottom")]
        public async Task<IActionResult> PostBasicWithDrawerBottom(
            [FromBody] CreateBasicWithDrawerBottomCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateBasicWithDrawerBottomAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("lcornerbottom")]
        public async Task<IActionResult> PostLCornerBottom([FromBody] CreateLCornerBottomCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateLCornerBottomAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("basictop")]
        public async Task<IActionResult> PostBasicTop([FromBody] CreateBasicTopCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateBasicTopAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("cutfinalbottom")]
        public async Task<IActionResult> PostCutFinalBottom([FromBody] CreateCutFinalBottomCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateCutFinalBottomAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("onehorizontaltop")]
        public async Task<IActionResult> PostOneHorizontalTop([FromBody] CreateOneHorizontalTopCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateOneHorizontalTopAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("twohorizontaltop")]
        public async Task<IActionResult> PostTwoHorizontalTop([FromBody] CreateTwoHorizontalTopCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateTwoHorizontalTopAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("threehorizontaltop")]
        public async Task<IActionResult> PostThreeHorizontalTop([FromBody] CreateThreeHorizontalTopCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateThreeHorizontalTopAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("basicglasstop")]
        public async Task<IActionResult> PostBasicGlassTop([FromBody] CreateBasicGlassTopCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _furnitureService.CreateBasicGlassTopAsync(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        [Route("onehorizontalglasstop")]
        public async Task<IActionResult> PostOneHorizontalGlassTop(
            [FromBody] CreateOneHorizontalGlassTopCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _furnitureService.CreateOneHorizontalGlassTopAsync(command);
            return Ok(ResponseDto.Default);
        }

        [HttpPost]
        [Route("twohorizontalglasstop")]
        public async Task<IActionResult> PostTwoHorizontalGlassTop(
            [FromBody] CreateTwoHorizontalGlassTopCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _furnitureService.CreateTwoHorizontalGlassTopAsync(command);
            return Ok(ResponseDto.Default);
        }

        [HttpPost]
        [Route("threehorizontalglasstop")]
        public async Task<IActionResult> PostThreeHorizontalGlassTop(
            [FromBody] CreateThreeHorizontalGlassTopCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _furnitureService.CreateThreeHorizontalGlassTopAsync(command);
            return Ok(ResponseDto.Default);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteFurniture(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _furnitureService.DeleteFurnitureAsync(new DeleteFurnitureCommand
            {
                Id = id
            });
            return Ok(ResponseDto.Default);
        }
    }
}