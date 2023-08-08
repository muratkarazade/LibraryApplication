using LibraryApplication.Models;
using LibraryApplication.Services;
using LibraryApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LibraryApplication.Controllers
{
    public class BorrowController : Controller
    {
        private readonly UserService _userService;
        private readonly BorrowService _borrowService;
        private readonly BookService _bookService;
        private readonly ILogger<BorrowController> _logger;

        /// <summary>
        /// BorrowController için constructor. Gerekli servisleri ve logger'ı enjekte eder.
        /// </summary>
        public BorrowController(UserService userService,
               BorrowService borrowService,
               BookService bookService,
               ILogger<BorrowController> logger)
        {
            _userService = userService;
            _borrowService = borrowService;
            _bookService = bookService;
            _logger = logger;
        }

        /// <summary>
        /// Belirtilen kitap ID'si için ödünç alma formunu döndürür.
        /// </summary>
        /// <param name="bookId">Ödünç alınacak kitap ID'si</param>
        /// <returns>Ödünç alma formu</returns>
        [HttpGet]
        [Route("Create/{bookId}")]
        public IActionResult Create(Guid bookId)
        {
            var viewModel = new BorrowViewModel
            {
                BookId = bookId
            };

            return View(viewModel);
        }

        /// <summary>
        /// Ödünç alma formundan gelen bilgilerle ödünç alma işlemini gerçekleştirir.
        /// </summary>
        /// <param name="model">Ödünç alma bilgileri</param>
        /// <returns>Kitap listesi sayfasına yönlendirme veya hata durumunda ödünç alma formunu geri döndürme</returns>
        [HttpPost]
        [Route("Create/{bookId}")]
        public async Task<IActionResult> Create(BorrowViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // Verilen email ile bir kullanıcı al ya da oluştur
                var user = await _userService.GetUserByEmail(model.Email);
                if (user == null)
                {
                    user = new User
                    {
                        Name = model.Name,
                        Surname = model.Surname,
                        Email = model.Email,
                        MobilePhone = model.PhoneNumber
                    };
                    await _userService.AddAsync(user);
                }

                // Yeni bir Borrow kaydı oluştur
                var borrow = new Borrow
                {
                    BookId = model.BookId,
                    UserId = user.Id,
                    BorrowDate = DateTime.Now,
                    ReturnDate = model.ReturnDate
                };
                await _borrowService.AddAsync(borrow);

                // Kitabı al ve ödünç alınmış olarak işaretle
                var book = await _bookService.GetByIdAsync(model.BookId);
                book.IsBorrowed = true;
                await _bookService.UpdateBookAsync(book);

                // Başarılı ödünç alma işleminden sonra kitap listesine yönlendir
                return RedirectToAction("Index", "Book");
            }
            catch (Exception ex)
            {
                // Hata oluştuğunda, hata bilgisini logla
                _logger.LogError(ex, "Borrow kaydı oluşturulurken bir hata oluştu.");

                // Hata mesajını ModelState'e ekle
                ModelState.AddModelError(string.Empty, $"Borrow kaydı oluşturulurken bir hata oluştu: {ex.Message}");

                // Hata durumunda ödünç alma formunu geri döndür
                return View(model);
            }
        }
    }
}
