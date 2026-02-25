using Library.Core.Entities;
using Library.Core.Enums;
using Library.Core.Exceptions;
using Library.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _repo;

        public BooksController(IBookRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index(string? search, Genre? genre)
        {
            var books = await _repo.SearchAsync(search, genre);
            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _repo.GetByIdAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            try
            {
                if (!ModelState.IsValid) return View(book);

                await _repo.AddAsync(book);
                TempData["Success"] = "Book added successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (DuplicateIsbnException ex)
            {
                ModelState.AddModelError("ISBN", ex.Message);
                return View(book);
            }
        }
    }
}
