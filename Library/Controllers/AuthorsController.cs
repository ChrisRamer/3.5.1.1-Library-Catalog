using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Library.Models;

namespace Library.Controllers
{
	public class AuthorsController : Controller
	{
		private readonly LibraryContext _db;

		public AuthorsController(LibraryContext db)
		{
			_db = db;
		}

		private Author GetAuthorFromId(int id)
		{
			return _db.Authors.FirstOrDefault(author => author.AuthorId == id);
		}

		public ActionResult Index()
		{
			List<Author> model = _db.Authors.ToList();
			return View(model);
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(Author author)
		{
			_db.Authors.Add(author);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		public ActionResult Details(int id)
		{
			Author thisAuthor = _db.Authors
				.Include(author => author.Books)
				.ThenInclude(join => join.Book)
				.FirstOrDefault(author => author.AuthorId == id);
			return View(thisAuthor);
		}

		public ActionResult Edit(int id)
		{
			Author thisAuthor = GetAuthorFromId(id);
			return View(thisAuthor);
		}

		[HttpPost]
		public ActionResult Edit(Author author)
		{
			_db.Entry(author).State = EntityState.Modified;
			_db.SaveChanges();
			return RedirectToAction("Details", new { id = author.AuthorId });
		}

		public ActionResult Delete(int id)
		{
			Author thisAuthor = GetAuthorFromId(id);
			return View(thisAuthor);
		}

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			Author thisAuthor = GetAuthorFromId(id);
			_db.Authors.Remove(thisAuthor);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		public ActionResult AddBook(int id)
		{
			Author thisAuthor = GetAuthorFromId(id);
			ViewBag.BookId = new SelectList(_db.Books, "BookId", "Name");
			return View(thisAuthor);
		}

		[HttpPost]
		public ActionResult AddBook(Author author, int bookId)
		{
			bool duplicate = _db.AuthorBooks.Any(join => join.AuthorId == author.AuthorId && join.BookId == bookId);

			if (bookId != 0 && !duplicate)
			{
				_db.AuthorBooks.Add(new AuthorBook() { AuthorId = author.AuthorId, BookId = bookId });
			}

			_db.SaveChanges();
			return RedirectToAction("Details", new { id = author.AuthorId });
		}
	}
}