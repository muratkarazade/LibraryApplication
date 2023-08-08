using System.ComponentModel.DataAnnotations;

namespace LibraryApplication.Models
{
    public class Borrow
    {
        [Key]
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
