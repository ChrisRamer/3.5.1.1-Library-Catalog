using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Library.Models;

namespace Library.Controllers
{
	public class BooksController : Controller
	{
		private readonly LibraryContext _db;

		public BooksController(LibraryContext db)
		{
			_db = db;
		}

		private Book GetBookFromId(int id)
		{
			return _db.Books.FirstOrDefault(book => book.BookId == id);
		}

		public ActionResult Index()
		{
			List<Book> model = _db.Books.ToList();
			return View(model);
		}

		public ActionResult Create()
		{
			ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
			return View();
		}

		[HttpPost]
		public ActionResult Create(Book book, int authorId)
		{
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