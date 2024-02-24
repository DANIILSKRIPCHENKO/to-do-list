using ToDoListManager.Domain.Entities;

namespace ToDoListManager.Domain.Agregates
{
    public class ToDoAgregate
    {
        public ToDoAgregate(ToDo todo, List<ToDoChangeLog> changeLogs)
        {
            ToDoId = todo.Id;
            Handle = todo.Handle;
            Description = todo.Description;
            Comments = todo.Comments;
            ToDoListId = todo.ToDoListId;
            Status = changeLogs.OrderBy(x => x.CreatedAt).Last().NewStatus;
            CreatedAt = changeLogs.OrderBy(x => x.CreatedAt).First().CreatedAt;
        }
        
        public Guid ToDoId { get; private set; }

        public string Handle { get; private set; }

        public string Description { get; private set; }

        public string Comments { get; private set; }

        public ToDoStatus Status { get; private set; }
        
        public DateTime CreatedAt { get; private set; }

        public Guid ToDoListId { get; private set; }
    }
}
