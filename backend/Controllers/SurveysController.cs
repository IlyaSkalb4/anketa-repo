using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
	public class SurveysController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
