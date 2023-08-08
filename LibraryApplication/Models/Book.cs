using System.ComponentModel.DataAnnotations;

namespace LibraryApplication.Models
{
    public class Book
    {
        [Key]

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImagePath { get; set; }
        public bool IsBorrowed { get; set; }
        public Borrow Borrow { get; set; }       
    }
}
