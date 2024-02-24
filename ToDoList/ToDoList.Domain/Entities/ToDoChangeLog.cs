namespace ToDoListManager.Domain.Entities
{
    public class ToDoChangeLog : EntityBase
    {
        public ToDoChangeLog(Guid id, Guid toDoId, ToDoStatus newStatus, DateTime createdAt)
        {
            Id = id;
            ToDoId = toDoId;
            NewStatus = newStatus;
            CreatedAt = createdAt;
        }

        public ToDoChangeLog(Guid toDoId, ToDoStatus newStatus)
        {
            Id = Guid.NewGuid();
            ToDoId = toDoId;
            NewStatus = newStatus;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid ToDoId { get; private set; }

        public ToDoStatus NewStatus { get; private set; }

        public DateTime CreatedAt { get; private set; }
    }

    public enum ToDoStatus
    {
        OnHold = 0,
        Active = 1,
        Resolved = 2,
    }
}
