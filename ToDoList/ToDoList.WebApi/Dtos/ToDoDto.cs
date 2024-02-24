using ToDoListManager.Domain.Entities;

namespace ToDoListManager.WebApi.Dtos
{
    public class ToDoDto
    {
        public Guid Id { get; set; }

        public string Handle { get; set; }

        public string Description { get; set; }

        public string Comments { get; set; }

        public ToDoStatus Status { get; set; }

        public Guid ToDoListId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
