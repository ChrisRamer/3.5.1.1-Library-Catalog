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

		public ActionResult Index()
		{
			List<Author> model = _db.Authors.ToList();
			return View(model);
		}
	}
}