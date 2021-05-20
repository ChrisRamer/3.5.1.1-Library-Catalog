using Microsoft.AspNetCore.Mvc;

namespace XXX_ProjectName.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet("/")]
		public ActionResult Index()
		{
			return View();
		}
	}
}