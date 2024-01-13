using Bookify.Core.Models;
using Bookify.Core.ViewModel;
using Bookify.Data;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Controllers
{
	public class CategoriesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CategoriesController(ApplicationDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var categories = _context.Categories.ToList();
			return View(categories);
		}
		[HttpGet]
        public IActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
		[ValidateAntiForgeryToken]
        public IActionResult Create(CreateCategoryViewModel model)
        {
			if(!ModelState.IsValid)
                return View(model);
			var category = new Category
			{
				Name = model.Name,
			};
			_context.Categories.Add(category);
			_context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
