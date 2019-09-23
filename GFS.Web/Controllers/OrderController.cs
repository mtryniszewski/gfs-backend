using System.Threading.Tasks;
using GFS.Domain.Core;
using GFS.Transfer.Order.Commands;
using GFS.Transfer.Order.Queries;
using GFS.Transfer.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GFS.Web.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Policy = "Standard")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _orderService.GetAsync(new GetOrderQuery
            {
                Id = id
            });

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "User")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ListOrderQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _orderService.ListAsync(query);

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "User")]
        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _orderService.ShowOrderDetailsAsync(new GetOrderQuery
            {
                Id = id
            });

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "Standard")]
        [HttpGet]
        [Route("archives")]
        public async Task<IActionResult> GetArchives([FromQuery] ListOrderQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _orderService.ListArchivesAsync(query);

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll([FromQuery] ListAllOrdersQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _orderService.ListAllAsync(query);

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        [Route("allarchives")]
        public async Task<IActionResult> GetAllArchives([FromQuery] ListAllOrdersQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _orderService.ListAllArchivesAsync(query);

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "Standard")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _orderService.CreateAsync(command);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "Standard")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromBody] UpdateOrderCommand command, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            command.Id = id;

            await _orderService.UpdateAsync(command);
            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "Standard")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteAsync(new DeleteOrderCommand
            {
                Id = id
            });

            return Ok(ResponseDto.Default);
        }


        [Authorize(Policy = "User")]
        [HttpPatch]
        [Route("confirm/{id}")]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            await _orderService.ConfirmAsync(new ManageOrderCommand
            {
                Id = id
            });

            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "User")]
        [HttpPatch]
        [Route("archive/{id}")]
        public async Task<IActionResult> Archive(int id)
        {
            await _orderService.ArchiveAsync(new ManageOrderCommand
            {
                Id = id
            });

            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "User")]
        [HttpPatch]
        [Route("dearchive/{id}")]
        public async Task<IActionResult> Dearchive(int id)
        {
            await _orderService.DearchiveAsync(new ManageOrderCommand
            {
                Id = id
            });

            return Ok(ResponseDto.Default);
        }
    }
}