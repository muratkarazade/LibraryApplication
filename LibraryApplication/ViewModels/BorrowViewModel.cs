namespace LibraryApplication.ViewModels
{
    public class BorrowViewModel
    {
        public Guid BookId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
