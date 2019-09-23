using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using GFS.Data.Model.Entities;
using GFS.Domain.Core;
using GFS.Transfer.Shared;
using GFS.Transfer.User.Commands;
using GFS.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GFS.Web.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IOptions<JwtAuthentication> _jwtAuthentication;
        private readonly IUserService _userService;

        public AuthController(IOptions<JwtAuthentication> jwtAuthentication,
            IUserService userService)
        {
            _jwtAuthentication = jwtAuthentication;
            _userService = userService;
        }


        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] LoginCommand model)
        {
            var user = await _userService.ValidateUser(model);
            if (user == null)
                return BadRequest();
            var token = GenerateToken(user);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            }.ToResponseDto());
        }


        private JwtSecurityToken GenerateToken(User userToVerify)
        {
            var token = new JwtSecurityToken(
                _jwtAuthentication.Value.ValidIssuer,
                _jwtAuthentication.Value.ValidAudience,
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userToVerify.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, userToVerify.Name),
                    new Claim(JwtRegisteredClaimNames.Sub, userToVerify.Surname),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, userToVerify.Id),
                    new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                        userToVerify.Permissions.ToString()),
                    new Claim("Permissions",
                        userToVerify.Permissions.ToString())

                },
                expires: DateTime.UtcNow.AddMinutes(120),
                notBefore: DateTime.UtcNow,
                signingCredentials: _jwtAuthentication.Value.SigningCredentials);
            return token;
        }
    }
}