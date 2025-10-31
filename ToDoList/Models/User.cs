namespace ToDoList.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int TotalTask => Tasks?.Count() ?? 0;
        public List<UserTask> Tasks { get; set; }

        public User(int id,string username, string password) 
        {
            Id = id;
            Username = username;
            Password = password;
            Tasks = new List<UserTask>();
        }

    }
}
