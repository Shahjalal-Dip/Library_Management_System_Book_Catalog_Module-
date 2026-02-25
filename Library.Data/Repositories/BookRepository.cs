using Library.Core.Entities;
using Library.Core.Enums;
using Library.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;
        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Book book)
        {
            if (!await IsIsbnUniqueAsync(book.ISBN))
                throw new DuplicateIsbnException(book.ISBN);

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id) ?? throw new BookNotFoundException(id);
           
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .FindAsync(id);
        }

        public async Task<bool> IsIsbnUniqueAsync(string isbn, int? excludeId = null)
        {
            return !await _context.Books
                .AnyAsync(b => b.ISBN == isbn && (!excludeId.HasValue || b.Id != excludeId));
        }

        public async Task<IEnumerable<Book>> SearchAsync(string? searchTerm = null, Genre? genre = null)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(b => b.Title.Contains(searchTerm) || b.ISBN.Contains(searchTerm));

            if (genre.HasValue)
                query = query.Where(b => b.Genre == genre.Value);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task UpdateAsync(Book book)
        {
           if (!await IsIsbnUniqueAsync(book.ISBN, book.Id))
                throw new DuplicateIsbnException(book.ISBN);

           book.LastUpdated = DateTime.UtcNow;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
    }
}
