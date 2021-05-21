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
	}
}