using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ToDoListManager.App.Commands;
using ToDoListManager.App.Errors;
using ToDoListManager.Domain.Entities;
using ToDoListManager.WebApi.Configuration;
using ToDoListManager.WebApi.Customization;
using ToDoListManager.WebApi.Dtos;
using ToDoListManager.WebApi.Models;

namespace ToDoListManager.WebApi.Controllers
{

    [ApiController]
    [StandardRoute]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly JwtConfiguration _jwtConfiguration;

        public UsersController(IMediator mediator, IOptionsSnapshot<JwtConfiguration> jwtConfiguration)
        {
            _mediator = mediator;
            _jwtConfiguration = jwtConfiguration.Value;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserDto>> Register(
            [FromBody] RegisterUserModel registerUserModel)
        {
            try
            {
                var user = await _mediator.Send(new RegisterUserCommand
                {
                    Email = registerUserModel.Email,
                    Name = registerUserModel.Name,
                    Password = registerUserModel.Password,
                });
                return Ok(MapUser(user));
            }
            catch (ConflictException)
            {
                return Conflict();
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> Login(
            [FromBody]LoginUserModel loginUserModel)
        {
            try
            {
                var loginUserQuery = new LoginUserQuery()
                {
                    Email = loginUserModel.Email,
                    Password = loginUserModel.Password,
                };
                var user = await _mediator.Send(loginUserQuery);

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var securityToken = new JwtSecurityToken(
                    _jwtConfiguration.Issuer,
                    _jwtConfiguration.Issuer,
                    new List<Claim> { new(ClaimTypes.NameIdentifier, user.Id.ToString()) },
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

                return Ok(token);
            }
            catch (EntityNotFoundException)
            {
                return Unauthorized();
            }
            catch (ConflictException)
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Update(
            [FromBody] UpdateUserModel updateUserModel)
        {
            try
            {
                var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var updateUserCommand = new UpdateUserCommand()
                {
                    Id = currentUserId,
                    Email = updateUserModel.Email,
                    Name = updateUserModel.Name,
                    Password = updateUserModel.Password
                };
                var user = await _mediator.Send(updateUserCommand);
                return Ok(MapUser(user));
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        private static UserDto MapUser(User user) => new()
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Password = user.Password
        };
    }
}
