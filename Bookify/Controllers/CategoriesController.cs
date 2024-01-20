﻿using Bookify.Core.Models;
using Bookify.Core.ViewModel;
using Bookify.Data;
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
			var categories = _context.Categories.AsNoTracking().ToList();
			return View(categories);
		}
		[HttpGet]
        public IActionResult Create()
        {
           
            return PartialView("Create");
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
			_context.Categories.Add(category);
			_context.SaveChanges();
            TempData["Message"] = "Item Created Successfuly!";
            return RedirectToAction("Index");
        }
        [HttpGet]
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

            return View(CategoryModel);
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

            return RedirectToAction("Index");
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
    }
}
