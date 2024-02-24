using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ToDoListManager.App.Commands;
using ToDoListManager.WebApi.Customization;
using ToDoListManager.WebApi.Dtos;
using ToDoListManager.WebApi.Models;
using MediatR;
using ToDoListManager.App.Queries;
using ToDoListManager.App.Errors;
using Microsoft.AspNetCore.Http;
using ToDoListManager.Domain.Agregates;

namespace ToDoListManager.WebApi.Controllers
{
    [ApiController]
    [StandardRoute("to-do")]
    public class ToDoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ToDoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ToDoDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ToDoDto>> Create(
            [FromBody] CreateToDoModel createToDoModel)
        {
            try
            {
                var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var createToDoCommand = new CreateToDoCommand()
                {
                    Handle = createToDoModel.Handle,
                    Description = createToDoModel.Description,
                    Comments = createToDoModel.Comments,
                    ToDoListId = createToDoModel.ToDoListId,
                    UserId = currentUserId
                };

                var createdToDo = await _mediator.Send(createToDoCommand);

                return Ok(MapToDo(createdToDo));
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidEntityOwnerException)
            {
                return Forbid();
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ToDoDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ToDoDto>> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateToDoModel updateToDoModel)
        {
            try
            {
                var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var updateToDoCommand = new UpdateToDoCommand()
                {
                    Id = id,
                    Handle = updateToDoModel.Handle,
                    Description = updateToDoModel.Description,
                    Comments = updateToDoModel.Comments,
                    Status = updateToDoModel.Status,
                    ToDoListId = updateToDoModel.ToDoListId,
                    UserId = currentUserId
                };

                var updatedToDo = await _mediator.Send(updateToDoCommand);

                return Ok(MapToDo(updatedToDo));
            }
            catch(EntityNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidEntityOwnerException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ToDoDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ToDoDto>> Delete(
            [FromRoute] Guid id)
        {
            try
            {
                var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var deleteToDoCommand = new DeleteToDoCommand()
                {
                    Id = id,
                    UserId = currentUserId
                };

                var updatedToDo = await _mediator.Send(deleteToDoCommand);

                return Ok(MapToDo(updatedToDo));
            }
            catch(EntityNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidEntityOwnerException)
            {
                return Forbid();
            }
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ToDoDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<ToDoDto>>> GetAll(
            [FromQuery] Guid toDoListId)
        {
            try
            {
                var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var getToDosQuery = new GetToDosByToDoListIdQuery()
                {
                    ToDoListId = toDoListId,
                    UserId = currentUserId
                };

                var toDos = await _mediator.Send(getToDosQuery);

                return Ok(toDos.Select(x => MapToDo(x)));
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidEntityOwnerException)
            {
                return Forbid();
            }
        }

        private static ToDoDto MapToDo(ToDoAgregate toDoAgregate) => new()
        {
            Id = toDoAgregate.ToDoId,
            Handle = toDoAgregate.Handle,
            Description = toDoAgregate.Description,
            Comments = toDoAgregate.Comments,
            Status = toDoAgregate.Status,
            ToDoListId = toDoAgregate.ToDoListId,
            CreatedAt = toDoAgregate.CreatedAt
        };
    }
}
