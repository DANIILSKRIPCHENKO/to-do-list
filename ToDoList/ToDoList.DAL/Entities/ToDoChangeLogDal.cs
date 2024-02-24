using ToDoListManager.Domain.Entities;

namespace ToDoListManager.DAL.Entities
{
    public class ToDoChangeLogDal
    {
        public Guid Id { get; set; }

        public Guid ToDoId { get; set; }

        public ToDoStatus NewStatus { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
