using System.Threading.Tasks;
using GFS.Domain.Core;
using GFS.Transfer.Producer.Commands;
using GFS.Transfer.Producer.Data;
using GFS.Transfer.Producer.Queries;
using GFS.Transfer.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GFS.Web.Controllers
{
    [Route("api/[controller]")]
    public class ProducerController : Controller
    {
        private readonly IProducerService _producerService;

        public ProducerController(IProducerService producerService)
        {
            _producerService = producerService;
        }

        [Authorize(Policy = "User")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

           var result=await _producerService.GetAsync(new GetProducerQuery
            {
                Id = id
            });

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "User")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ListProducerQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result= await _producerService.ListAsync(query);

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "User")]
        [HttpGet]
        [Route("simple")]
        public async Task<IActionResult> GetSimple([FromQuery] ListProducerQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _producerService.ListSimpleAsync(query);

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "User")]
        [HttpGet]
        [Route("archives")]
        public async Task<IActionResult> GetArchives([FromQuery] ListProducerQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _producerService.ListArchivesAsync(query);

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProducerCommand producerCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest();

           var result= await _producerService.CreateAsync(producerCommand);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "Admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromBody] UpdateProducerCommand producerCommand, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            producerCommand.Id = id;

            await _producerService.UpdateAsync(producerCommand);
            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _producerService.DeleteAsync(new DeleteProducerCommand
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
            await _producerService.ArchiveAsync(new ManageProducerCommand
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
            await _producerService.DearchiveAsync(new ManageProducerCommand
            {
                Id = id
            });

            return Ok(ResponseDto.Default);
        }
    }
}