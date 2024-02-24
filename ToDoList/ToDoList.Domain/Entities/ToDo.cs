namespace ToDoListManager.Domain.Entities
{
    public class ToDo: EntityBase
    {
        public ToDo(string handle, string description, string comments, Guid toDoListId)
        {
            Id = Guid.NewGuid();
            Handle = handle;
            Description = description;
            Comments = comments;
            ToDoListId = toDoListId;
        }

        public ToDo(Guid id, string handle, string description, string comments, Guid toDoListId)
        {
            Id = id;
            Handle = handle;
            Description = description;
            Comments = comments;
            ToDoListId = toDoListId;
        }

        public string Handle { get; private set; }

        public string Description { get; private set; }

        public string Comments { get; private set; }

        public Guid ToDoListId { get; private set; }
    }
}
