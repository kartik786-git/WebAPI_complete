namespace WebAPI.VeiwModel
{
    public class MultipleUploadModel
    {
        public List<IFormFile> Files { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }
}
