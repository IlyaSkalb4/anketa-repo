using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
	public class QuestionnaireController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
