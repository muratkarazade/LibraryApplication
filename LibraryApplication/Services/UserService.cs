using LibraryApplication.Data;
using LibraryApplication.Interfaces;
using LibraryApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibraryApplication.Services
{
    public class UserService : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserService> _logger;

        /// <summary>
        /// UserService için constructor. Context ve logger servislerini ayarlar.
        /// </summary>
        public UserService(AppDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Kullanıcıyı veritabanına ekler.
        /// </summary>
        public async Task AddAsync(User entity)
        {
            try
            {
                _context.Users.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var message = $"Kullanıcı eklenirken bir hata oluştu. Kullanıcı Adı: {entity.Name} {entity.Surname}";
                _logger.LogError(ex, message);
                throw;
            }
        }

        /// <summary>
        /// Verilen ifadeye göre kullanıcıları filtreler.
        /// </summary>
        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> expression)
        {
            try
            {
                return await _context.Users.Where(expression).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcılar filtrelenirken bir hata oluştu.");
                throw;
            }
        }

        /// <summary>
        /// Tüm kullanıcıları getirir.
        /// </summary>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm kullanıcılar getirilirken bir hata oluştu.");
                throw;
            }
        }

        /// <summary>
        /// Verilen ID'ye göre kullanıcıyı getirir.
        /// </summary>
        public async Task<User> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Users.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{id} ID'li kullanıcı getirilirken bir hata oluştu.");
                throw;
            }
        }

        /// <summary>
        /// Verilen email adresine göre kullanıcıyı getirir.
        /// </summary>
        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    var message = $"Email adresi {email} olan kullanıcı bulunamadı.";
                    _logger.LogWarning(message); // Log seviyesini Warning olarak değiştirdim, çünkü bu beklenen bir durum olabilir.
                }
                return user;
            }
            catch (Exception ex)
            {
                var message = $"{email} email adresine sahip kullanıcı getirilirken beklenmeyen bir hata oluştu.";
                _logger.LogError(ex, message);
                throw;
            }
        }
    }
}
