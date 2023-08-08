using LibraryApplication.Interfaces;
using LibraryApplication.Models;
using LibraryApplication.Services;
using LibraryApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LibraryApplication.Controllers
{
    public class BookController : Controller
    {
        // Kitap servisi için özel alan (dependency injection ile set edilir)
        private readonly BookService _bookService;

        /// <summary>
        /// BookController için constructor. Bağımlılıkları enjekte eder.
        /// </summary>
        /// <param name="bookService">Kitap servisi</param>
        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Tüm kitapların listesini döndürür. Kitap ve ödünç alma bilgileri ile birlikte.
        /// </summary>
        /// <returns>Kitap listesi ve ödünç bilgisi</returns>
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllAsync();
            var viewModels = books.Select(b => new BookBorrowViewModel
            {
                Book = b,
                Borrow = b.Borrow // Book içinde bulunan Borrow nesnesini al
            }).ToList();

            return View(viewModels);
        }

        /// <summary>
        /// Yeni kitap oluşturma sayfasını döndürür.
        /// </summary>
        /// <returns>Yeni kitap oluşturma sayfası</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Yeni kitabı sisteme ekler. Başarılı olursa ana sayfaya yönlendirir.
        /// </summary>
        /// <param name="bookViewModel">Yeni kitap bilgileri</param>
        /// <returns>Kitap listesi sayfası veya hata mesajı ile kitap oluşturma sayfası</returns>
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromForm] CreateBookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _bookService.AddBookAsync(bookViewModel);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Hata oluştuğunda kullanıcıya hata mesajı göster
                    ModelState.AddModelError("", "Kitap eklenirken bir hata oluştu.");
                }
            }
            return View(bookViewModel);
        }
    }
}
