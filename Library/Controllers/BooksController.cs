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
	}
}