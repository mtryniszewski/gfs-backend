using System.Threading.Tasks;
using GFS.Domain.Core;
using GFS.Transfer.Shared;
using GFS.Transfer.User.Commands;
using GFS.Transfer.User.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GFS.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [Authorize(Policy = "User")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _userService.GetAsync(new GetUserQuery
            {
                Id = id
            });

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ListUserQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _userService.ListAsync(query);

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        [Route("archives")]
        public async Task<IActionResult> GetArchives([FromQuery] ListUserQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _userService.ListArchivesAsync(query);

            return Ok(result.ToResponseDto());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserCommand userCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _userService.CreateAsync(userCommand);

            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "User")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromBody] UpdateUserCommand userCommand, string id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            userCommand.Id = id;
            await _userService.UpdateAsync(userCommand);

            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "Standard")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _userService.DeleteAsync(new DeleteUserCommand
            {
                Id = id
            });

            return Ok(ResponseDto.Default);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("activateaccount")]
        public async Task<IActionResult> ActivateByLink([FromBody] ActivateAccountCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _userService.ActivateByLinkAsync(command);

            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "User")]
        [HttpPatch]
        [Route("changepassword")]
        public async Task<IActionResult> ChangePasswordInProfile([FromBody] ChangePasswordCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _userService.ChangePasswordInProfileAsync(command);

            return Ok(ResponseDto.Default);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _userService.ForgotPasswordAsync(command);

            return Ok(ResponseDto.Default);
        }

        [AllowAnonymous]
        [HttpPatch]
        [Route("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _userService.ResetPasswordAsync(command);

            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch]
        [Route("grant/{id}")]
        public async Task<IActionResult> Grant(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _userService.GrantAsync(new ManageUserCommand
            {
                Id = id
            });

            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch]
        [Route("revoke/{id}")]
        public async Task<IActionResult> Revoke(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _userService.RevokeAsync(new ManageUserCommand
            {
                Id = id
            });

            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch]
        [Route("activate/{id}")]
        public async Task<IActionResult> Activate(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _userService.ActivateAsync(new ManageUserCommand
            {
                Id = id
            });

            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch]
        [Route("deactivate/{id}")]
        public async Task<IActionResult> Deactivate(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _userService.DeactivateAsync(new ManageUserCommand
            {
                Id = id
            });

            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch]
        [Route("archive/{id}")]
        public async Task<IActionResult> Archive(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _userService.ArchiveAsync(new ManageUserCommand
            {
                Id = id
            });

            return Ok(ResponseDto.Default);
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch]
        [Route("dearchive/{id}")]
        public async Task<IActionResult> Dearchive(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _userService.DearchiveAsync(new ManageUserCommand
            {
                Id = id
            });

            return Ok(ResponseDto.Default);
        }
    }
}