using Library.Core.Entities;
using Library.Core.Enums;
using Library.Core.Exceptions;
using Library.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _repository;

        public BooksController(IBookRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index(string? search, Genre? genre)
        {
            var books = await _repository.SearchAsync(search, genre);
            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _repository.GetByIdAsync(id);
            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(book);

                await _repository.AddAsync(book);
                TempData["Success"] = "Book added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DuplicateIsbnException ex)
            {
                ModelState.AddModelError("ISBN", ex.Message);
                return View(book);
            }
            catch (InvalidPublicationYearException ex)
            {
                ModelState.AddModelError("PublicationYear", ex.Message);
                return View(book);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var book = await _repository.GetByIdAsync(id);
            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book book)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(book);

                await _repository.UpdateAsync(book);
                TempData["Success"] = "Book updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DuplicateIsbnException ex)
            {
                ModelState.AddModelError("ISBN", ex.Message);
                return View(book);
            }
            catch (BookNotFoundException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _repository.GetByIdAsync(id);
            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                TempData["Success"] = "Book deleted successfully.";
            }
            catch (BookNotFoundException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}