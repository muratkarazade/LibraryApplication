using LibraryApplication.Data;
using LibraryApplication.Interfaces;
using LibraryApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibraryApplication.Services
{
    public class BorrowService : IBorrowRepository
    {
        private readonly AppDbContext _context;
        public BorrowService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Borrow entity)
        {
            _context.Borrows.Add(entity);
            await _context.SaveChangesAsync();
        }     

        public async Task<IEnumerable<Borrow>> FindAsync(Expression<Func<Borrow, bool>> expression)
        {
            return await _context.Borrows.Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<Borrow>> GetAllAsync()
        {
            return await _context.Borrows.ToListAsync();
        }

        public async Task<Borrow> GetByIdAsync(Guid id)
        {
            return await _context.Borrows.FindAsync(id);
        }      
    }
}
