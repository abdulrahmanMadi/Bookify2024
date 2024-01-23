using Bookify.Core.Models;
using Bookify.Core.ViewModel;
using Bookify.Data;
using Bookify.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
			var categories = _context.Categories.AsNoTracking().Select(
                x=> new CategoryViewModel 
                {
                    id=x.id,
                    Name=x.Name,
                    IsDeleted=x.IsDeleted,
                    CreatedOn=x.CreatedOn,
                    LastUpdatedOn = x.LastUpdatedOn
                }
                ).ToList();
			return View(categories);
		}
		[HttpGet]
        [AjaxOnly]

        public IActionResult Create()
        {
           
            return PartialView("_Form");
        }
        [HttpPost]
		[ValidateAntiForgeryToken]
        public IActionResult Create(CategoryFormViewModel model)
        {
			if(!ModelState.IsValid)
                return View(model);
			var category = new Category
			{
				Name = model.Name,
			};
            var viewModel = new CategoryViewModel
            {

                id = category.id,
                Name = category.Name,
                IsDeleted = category.IsDeleted,
                CreatedOn = category.CreatedOn,
                LastUpdatedOn = category.LastUpdatedOn

            };
			_context.Categories.Add(category);
			_context.SaveChanges();
            TempData["Message"] = "Item Created Successfuly!";
            return RedirectToAction("Index", viewModel);
        }
        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
                return NotFound();
            var CategoryModel= new CategoryFormViewModel 
            {
                Id = id,
                Name = category.Name, 
            };

            return PartialView("_Form",CategoryModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var category = _context.Categories.Find(model.Id);
            if (category == null)
                return NotFound();
            category.Name = model.Name;
            category.LastUpdatedOn=DateTime.Now;
            _context.SaveChanges();
            TempData["Message"] = "Item Updated Successfuly!";
            var viewModel = new CategoryViewModel
            {

                id = category.id,
                Name = category.Name,
                IsDeleted = category.IsDeleted,
                CreatedOn = category.CreatedOn,
                LastUpdatedOn = category.LastUpdatedOn

            };
            return RedirectToAction("Index", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult ToggleStatus(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
                return NotFound();
            category.IsDeleted = !category.IsDeleted;
            category.LastUpdatedOn=DateTime.Now;

            _context.SaveChanges();
            return Ok(category.LastUpdatedOn.ToString());
        }
        public IActionResult AllowItem(CategoryFormViewModel model)
        {
            var IsExist = _context.Categories.SingleOrDefault(x=>x.Name ==model.Name);
            var isAllowed = IsExist is null || IsExist.id.Equals(model.Id);
            return Json(isAllowed);
        }
    }
}
