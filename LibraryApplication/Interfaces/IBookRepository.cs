using LibraryApplication.Models;

namespace LibraryApplication.Interfaces
{
    public interface IBookRepository : IGenericRepository<Book>
    {    
        Task<Book> UpdateBookAsync(Book book);
    }
}
