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
	public class AuthorsController : Controller
	{
		private readonly LibraryContext _db;
		private readonly UserManager<ApplicationUser> _userManager;

		public AuthorsController(UserManager<ApplicationUser> userManager, LibraryContext db)
		{
			_userManager = userManager;
			_db = db;
		}

		private Author GetAuthorFromId(int id)
		{
			return _db.Authors.FirstOrDefault(author => author.AuthorId == id);
		}

		private async Task<ApplicationUser> GetCurrentUser()
		{
			string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			return await _userManager.FindByIdAsync(userId);
		}

		public async Task<ActionResult> Index()
		{
			ApplicationUser currentUser = await GetCurrentUser();
			List<Author> userAuthors = _db.Authors.Where(entry => entry.User.Id == currentUser.Id).ToList();
			return View(userAuthors);
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Create(Author author)
		{
			author.User = await GetCurrentUser();

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

		[HttpPost]
		public ActionResult DeleteBook(int joinId)
		{
			AuthorBook joinEntry = _db.AuthorBooks.FirstOrDefault(entry => entry.AuthorBookId == joinId);
			_db.AuthorBooks.Remove(joinEntry);
			_db.SaveChanges();
			return RedirectToAction("Details", new { id = joinEntry.AuthorId });
		}
	}
}