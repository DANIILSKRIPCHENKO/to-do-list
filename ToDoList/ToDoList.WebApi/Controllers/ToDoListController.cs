using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoListManager.App.Commands;
using ToDoListManager.App.Errors;
using ToDoListManager.App.Queries;
using ToDoListManager.Domain.Entities;
using ToDoListManager.WebApi.Customization;
using ToDoListManager.WebApi.Dtos;
using ToDoListManager.WebApi.Models;

namespace ToDoListManager.WebApi.Controllers
{
    [ApiController]
    [StandardRoute("to-do-list")]
    public class ToDoListController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ToDoListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ToDoListDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ToDoListDto>> Create(
            [FromBody]CreateToDoListModel createToDolistModel)
        {
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var createToDoListCommand = new CreateToDoListCommand()
            {
                Handle = createToDolistModel.Handle,
                Description = createToDolistModel.Description,
                UserId = currentUserId
            };

            var createdToDoList = await _mediator.Send(createToDoListCommand);
            return Ok(MapToDoList(createdToDoList));
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ToDoListDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ToDoListDto>> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateToDoListModel updateToDoListModel)
        {
            try
            {
                var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var updateToDoListCommand = new UpdateToDoListCommand()
                {
                    Id = id,
                    Handle = updateToDoListModel.Handle,
                    Description = updateToDoListModel.Description,
                    UserId = currentUserId
                };

                var updatedToDoList = await _mediator.Send(updateToDoListCommand);
                return Ok(MapToDoList(updatedToDoList));
            }
            catch(EntityNotFoundException)
            {
                return NotFound();
            }
            catch(InvalidEntityOwnerException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ToDoListDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ToDoListDto>> Delete([FromRoute] Guid id)
        {
            try
            {
                var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var updateToDoListCommand = new DeleteToDoListCommand()
                {
                    Id = id,
                    UserId = currentUserId
                };

                var deletedToDoList = await _mediator.Send(updateToDoListCommand);
                return Ok(MapToDoList(deletedToDoList));

            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidEntityOwnerException)
            {
                return Forbid();
            }
            catch (ConflictException)
            {
                return Conflict();
            }
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ToDoListDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ToDoListDto>>> GetAll()
        {
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var getToDoListByIdQuery = new GetToDoListsByUserIdQuery()
            {
                UserId = currentUserId
            };

            var toDoLists = await _mediator.Send(getToDoListByIdQuery);
            
            var toDoListDtos = toDoLists.Select(x => MapToDoList(x));
            
            return Ok(toDoListDtos);
        }

        private static ToDoListDto MapToDoList(ToDoList toDoList) => new()
        {
            Id = toDoList.Id,
            Handle = toDoList.Handle,
            Description = toDoList.Description,
        };
    }
}
