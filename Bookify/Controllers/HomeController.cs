using Bookify.Data;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}


        public IActionResult Index()
        {
            
            return View();
        }

      
    }
}
