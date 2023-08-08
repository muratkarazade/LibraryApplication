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
    public class BorrowService : IBorrowRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BorrowService> _logger;

        /// <summary>
        /// BorrowService için constructor. Gerekli context ve logger servislerini enjekte eder.
        /// </summary>
        public BorrowService(AppDbContext context, ILogger<BorrowService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Yeni bir ödünç işlemi ekler.
        /// </summary>
        public async Task AddAsync(Borrow entity)
        {
            try
            {
                _context.Borrows.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ödünç işlemi eklenirken hata oluştu.");
                throw;
            }
        }

        /// <summary>
        /// Belirtilen ifadeye göre ödünç işlemlerini filtreler ve döndürür.
        /// </summary>
        public async Task<IEnumerable<Borrow>> FindAsync(Expression<Func<Borrow, bool>> expression)
        {
            try
            {
                return await _context.Borrows.Where(expression).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ödünç işlemleri filtrelenirken hata oluştu.");
                throw;
            }
        }

        /// <summary>
        /// Tüm ödünç işlemlerini getirir.
        /// </summary>
        public async Task<IEnumerable<Borrow>> GetAllAsync()
        {
            try
            {
                return await _context.Borrows.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm ödünç işlemleri getirilirken hata oluştu.");
                throw;
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip ödünç işlemini getirir.
        /// </summary>
        public async Task<Borrow> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Borrows.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{id} ID'li ödünç işlemi getirilirken hata oluştu.");
                throw;
            }
        }
    }
}
