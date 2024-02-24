namespace ToDoListManager.Domain.Entities
{
    public class User : EntityBase
    {
        public User(string name, string email, string password)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Email = email;
            this.Password = password;
        }

        public User(Guid id, string name, string email, string password)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.Password = password;
        }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }
    }
}
