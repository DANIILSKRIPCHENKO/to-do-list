namespace ToDoListManager.WebApi.Models
{
    public class CreateToDoModel
    {
        public string Handle { get; set; }

        public string Description { get; set; }

        public string Comments { get; set; }

        public Guid ToDoListId { get; set; }
    }
}
