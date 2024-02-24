namespace ToDoListManager.DAL.Entities
{
    public class ToDoListDal
    {
        public Guid Id { get; set; }

        public string Handle { get; set; }

        public string Description { get; set; }

        public Guid UserId { get; set; }
    }
}
