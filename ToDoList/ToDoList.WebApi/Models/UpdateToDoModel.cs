using ToDoListManager.Domain.Entities;

namespace ToDoListManager.WebApi.Models
{
    public class UpdateToDoModel
    {
        public string Handle { get; set; }

        public string Description { get; set; }

        public string Comments { get; set; }

        public ToDoStatus Status { get; set; }

        public Guid ToDoListId { get; set; }
    }
}
