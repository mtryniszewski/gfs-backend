using System.Threading.Tasks;
using GFS.Domain.Core;
using GFS.Transfer.Fabric.Commands;
using GFS.Transfer.Fabric.Queries;
using GFS.Transfer.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GFS.Web.Controllers
{
    [Route("api/[controller]")]
    public class FabricController : Controller
    {
        private readonly IFabricService _fabricService;

        public FabricController(IFabricService fabricService)
        {
            _fabricService = fabricService;
        }

        [Authorize(Policy = "User")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ListFabricQuery query)
        {
            var result = await _fabricService.ListAsync(query);

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "User")]
        [HttpGet]
        [Route("archives")]
        public async Task<IActionResult> GetArchives([FromQuery] ListFabricQuery query)
        {
            var result = await _fabricService.ListArchivesAsync(query);

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "User")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _fabricService.GetAsync(new GetFabricQuery
            {
                Id = id
            });

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateFabricCommand fabricCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _fabricService.CreateAsync(fabricCommand);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "Admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromBody] UpdateFabricCommand fabricCommand, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            fabricCommand.Id = id;

            await _fabricService.UpdateAsync(fabricCommand);
            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _fabricService.DeleteAsync(new DeleteFabricCommand
            {
                Id = id
            });
            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch]
        [Route("archive/{id}")]
        public async Task<IActionResult> Archive(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _fabricService.ArchiveAsync(new ManageFabricCommand
            {
                Id = id
            });
            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch]
        [Route("dearchive/{id}")]
        public async Task<IActionResult> Dearchive(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _fabricService.DearchiveAsync(new ManageFabricCommand
            {
                Id = id
            });
            return Ok(ResponseDto.Default);
        }
    }
}