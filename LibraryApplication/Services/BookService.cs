using LibraryApplication.Data;
using LibraryApplication.Interfaces;
using LibraryApplication.Models;
using LibraryApplication.Services;
using LibraryApplication.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class BookService : IBookRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<BookService> _logger;
    private readonly PhotoService _photoService;

    /// <summary>
    /// BookService için constructor. Gerekli context, logger ve servisleri enjekte eder.
    /// </summary>
    public BookService(AppDbContext context, ILogger<BookService> logger, PhotoService photoService)
    {
        _context = context;
        _logger = logger;
        _photoService = photoService;
    }

    /// <summary>
    /// Kitap eklemek için metot. Fotoğrafı Cloudinary'e yükler ve kitabı veritabanına ekler.
    /// </summary>
    public async Task AddBookAsync(CreateBookViewModel bookViewModel)
    {
        try
        {
            // Yeni bir kitap oluşturur
            var entity = new Book
            {
                Title = bookViewModel.Title,
                Author = bookViewModel.Author
            };

            if (bookViewModel.PhotoFile != null)
            {
                // Yüklenen fotoğrafın Cloudinary URL'sini alır
                var uploadResult = await _photoService.AddPhotoAsync(bookViewModel.PhotoFile);
                var photoUrl = uploadResult?.SecureUrl.ToString();

                // Fotoğraf URL'sini kitap bilgileriyle birleştirir
                entity.ImagePath = photoUrl;
            }

            // Kitabı veritabanına ekler
            _context.Books.Add(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var message = $"Kitap eklenirken bir hata oluştu. Kitap adı: {bookViewModel.Title}";
            _logger.LogError(ex, message);
            throw new Exception($"{message}. Hata detayı: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Bir dizi kitabı veritabanına ekler.
    /// </summary>
    public async Task AddRangeAsync(IEnumerable<Book> entities)
    {
        try
        {
            _context.Books.AddRange(entities);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Birden fazla kitap eklenirken bir hata oluştu.");
            throw;
        }
    }

    /// <summary>
    /// Belirtilen ifadeye göre kitapları filtreler ve döndürür.
    /// </summary>
    public async Task<IEnumerable<Book>> FindAsync(Expression<Func<Book, bool>> expression)
    {
        try
        {
            return await _context.Books.Where(expression).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Kitaplar belirtilen ifadeye göre filtrelenirken bir hata oluştu.");
            throw;
        }
    }

    /// <summary>
    /// Tüm kitapları alfa sırasına göre getirir. İlişkili Borrow ve User bilgilerini de içerir.
    /// </summary>
    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        try
        {
            return await _context.Books
             .Include(b => b.Borrow)
                 .ThenInclude(br => br.User)
             .OrderBy(b => b.Title) // Title'a göre alfabetik sıralama
             .ToListAsync();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Tüm kitaplar getirilirken bir hata oluştu.");
            throw;
        }
    }

    /// <summary>
    /// Belirtilen ID'ye sahip kitabı getirir.
    /// </summary>
    public async Task<Book> GetByIdAsync(Guid id)
    {
        try
        {
            return await _context.Books.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{id} ID'li kitap getirilirken bir hata oluştu.");
            throw;
        }
    }

    /// <summary>
    /// Belirtilen kitabı günceller.
    /// </summary>
    public async Task<Book> UpdateBookAsync(Book book)
    {
        try
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }
        catch (Exception ex)
        {
            var message = $"Kitap {book.Title} güncellenirken beklenmeyen bir hata oluştu.";
            _logger.LogError(ex, message);
            throw new Exception(message);
        }
    }

    /// <summary>
    /// Belirtilen kitabın 'IsBorrowed' özelliğini günceller.
    /// </summary>
    public async Task<Book> UpdateBookIsBorrowedAsync(Guid bookId, bool isBorrowed)
    {
        try
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                var message = $"ID'si {bookId} olan kitap bulunamadı.";
                _logger.LogError(message);
                throw new Exception(message);
            }

            book.IsBorrowed = isBorrowed;

            await UpdateBookAsync(book);
            return book;
        }
        catch (Exception ex)
        {
            var message = $"Kitap {bookId} 'IsBorrowed' özelliği güncellenirken beklenmeyen bir hata oluştu.";
            _logger.LogError(ex, message);
            throw new Exception(message);
        }
    }

   
    public Task AddAsync(Book entity)
    {
        throw new NotImplementedException();
    }
}
