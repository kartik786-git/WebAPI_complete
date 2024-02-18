namespace WebAPI.VeiwModel
{
    public class FileUploadModel
    {
        public IFormFile File { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }
    
}
