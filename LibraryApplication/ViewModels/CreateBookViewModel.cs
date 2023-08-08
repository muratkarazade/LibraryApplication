namespace LibraryApplication.ViewModels
{
    public class CreateBookViewModel
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public IFormFile? PhotoFile { get; set; }
    }
}
