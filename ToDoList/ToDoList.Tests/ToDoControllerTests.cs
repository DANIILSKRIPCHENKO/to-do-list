using Castle.Components.DictionaryAdapter;
using FluentAssertions;
using MediatR;
using Moq;
using ToDoListManager.App.CommandHandlers;
using ToDoListManager.App.Commands;
using ToDoListManager.App.Errors;
using ToDoListManager.App.Interfaces.Repositories;

namespace ToDoListManager.Tests
{
    public class ToDoControllerTests
    {
        [Fact]
        public async void CreateToDoCommandHandler_ShouldThowEnityNotFoundException_When_GetByIdOrThrowAsync_Throws()
        {
            // Arrange
            var toDoRepository = new Mock<IToDoRepository>();
            var toDoListRepository = new Mock<IToDoListRepository>();
            var toDoChangeLogRepository = new Mock<IToDoChangeLogRepository>();

            toDoListRepository
                .Setup(x => x.GetByIdOrThrowAsync(It.IsAny<Guid>())).
                ThrowsAsync(new EntityNotFoundException());

            var command = new CreateToDoCommand()
            {
                Handle = "Test",
                Description = "Test",
                Comments = "Test",
                Status = Domain.Entities.ToDoStatus.OnHold,
                ToDoListId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var handler = new CreateToDoCommandHandler(
                toDoRepository.Object, 
                toDoListRepository.Object, 
                toDoChangeLogRepository.Object);

            // Act
            var act = async () => await handler.Handle(command, new CancellationToken());

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
        }
    }
}