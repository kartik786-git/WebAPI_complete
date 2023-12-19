namespace WebAPI.Entity
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int BlogId { get; set; }

    }
}
