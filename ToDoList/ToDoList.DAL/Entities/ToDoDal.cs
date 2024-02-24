namespace ToDoListManager.DAL.Entities
{
    public class ToDoDal
    {
        public Guid Id { get; set; }

        public string Handle { get; set; }

        public string Description { get; set; }

        public string Comments { get; set; }

        public Guid ToDoListId { get; set; }
    }
}
