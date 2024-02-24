namespace ToDoListManager.Domain.Entities
{
    public class ToDoList: EntityBase
    {
        public ToDoList(string handle, string description, Guid userId)
        {
            Id = Guid.NewGuid();
            Handle = handle;
            Description = description;
            UserId = userId;
        }

        public ToDoList(Guid id, string handle, string description, Guid userId)
        {
            Id = id;
            Handle = handle;
            Description = description;
            UserId = userId;
        }

        public string Handle { get; private set; }

        public string Description { get; private set; }

        public Guid UserId { get; set; }
    }
}
