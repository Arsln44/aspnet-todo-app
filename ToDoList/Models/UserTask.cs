namespace ToDoList.Models
{
    public class UserTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }

        public UserTask (int id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
            CreationTime = DateTime.Now;
        }
    }
}
