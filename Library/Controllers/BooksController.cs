using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Controllers
{
	[Authorize]
	public class BooksController : Controller
	{
		private readonly LibraryContext _db;
		private readonly UserManager<ApplicationUser> _userManager;

		public BooksController(UserManager<ApplicationUser> userManager, LibraryContext db)
		{
			_userManager = userManager;
			_db = db;
		}

		private Book GetBookFromId(int id)
		{
			return _db.Books.FirstOrDefault(book => book.BookId == id);
		}

		private async Task<ApplicationUser> GetCurrentUser()
		{
			string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			return await _userManager.FindByIdAsync(userId);
		}

		public async Task<ActionResult> Index()
		{
			ApplicationUser currentUser = await GetCurrentUser();
			List<Book> userBooks = _db.Books.Where(entry => entry.User.Id == currentUser.Id).ToList();
			return View(userBooks);
		}

		public ActionResult Create()
		{
			ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Create(Book book, int authorId)
		{
			book.User = await GetCurrentUser();
			_db.Books.Add(book);

			if (authorId != 0)
			{
				_db.AuthorBooks.Add(new AuthorBook() { AuthorId = authorId, BookId = book.BookId });
			}

			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		public ActionResult Details(int id)
		{
			Book thisBook = GetBookFromId(id);
			return View(thisBook);
		}

		public ActionResult Edit(int id)
		{
			Book thisBook = GetBookFromId(id);
			ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
			return View(thisBook);
		}

		[HttpPost]
		public ActionResult Edit(Book book, int authorId)
		{
			bool duplicate = _db.AuthorBooks.Any(join => join.AuthorId == authorId && join.BookId == book.BookId);

			if (authorId != 0 && !duplicate)
			{
				_db.AuthorBooks.Add(new AuthorBook() { AuthorId = authorId, BookId = book.BookId });
			}

			_db.Entry(book).State = EntityState.Modified;
			_db.SaveChanges();
			return RedirectToAction("Details", new { id = book.BookId });
		}

		public ActionResult Delete(int id)
		{
			Book thisBook = GetBookFromId(id);
			return View(thisBook);
		}

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			Book thisBook = GetBookFromId(id);
			_db.Books.Remove(thisBook);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		public ActionResult AddAuthor(int id)
		{
			Book thisBook = GetBookFromId(id);
			ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
			return View(thisBook);
		}

		[HttpPost]
		public ActionResult AddAuthor(Book book, int authorId)
		{
			bool duplicate = _db.AuthorBooks.Any(join => join.AuthorId == authorId && join.BookId == book.BookId);

			if (authorId != 0 && !duplicate)
			{
				_db.AuthorBooks.Add(new AuthorBook() { AuthorId = authorId, BookId = book.BookId });
			}

			_db.SaveChanges();
			return RedirectToAction("Details", new { id = book.BookId});
		}

		[HttpPost]
		public ActionResult DeleteAuthor(int joinId)
		{
			AuthorBook joinEntry = _db.AuthorBooks.FirstOrDefault(entry => entry.AuthorBookId == joinId);
			_db.AuthorBooks.Remove(joinEntry);
			_db.SaveChanges();
			return RedirectToAction("Details", new { id = joinEntry.AuthorId });
		}
	}
}