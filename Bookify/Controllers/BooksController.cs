using Bookify.Core.Consts;
using Bookify.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Controllers
{
	public class BooksController : Controller
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		private List<string> _allowedExtentions = new (){ ".jpg",".png", ".jpeg" };
		private int _maxSize=2097152;

        public BooksController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public BookFormViewModel PopluteBookFormViewModel(BookFormViewModel? model = null)
        {
            BookFormViewModel viewmodel = model is null ? new BookFormViewModel() : model;
            var categoies = _context.Categories.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList();
            var authors = _context.Authors.Where(x => !x.IsDeleted).OrderBy(x => x.Name).ToList();
            viewmodel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);
            viewmodel.Catgegories = _mapper.Map<IEnumerable<SelectListItem>>(categoies);
            return viewmodel;
        }
        public IActionResult Index()
		{
			return View();
		}
		public IActionResult Create()
		{
			

			var viewModel = PopluteBookFormViewModel();

            return View("Form", viewModel);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
        public IActionResult Create(BookFormViewModel model)
        {
			if (!ModelState.IsValid)
			{
				model = PopluteBookFormViewModel(model);
                return View("Form", model);
            }
          
            var books= _mapper.Map<Book>(model);
            if (model.Image is not null)
            {
                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedExtentions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Error.NotAllowedExtention);
                    return View("Form",PopluteBookFormViewModel(model));
                }
                if(model.Image.Length > _maxSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Error.NotAllowedSize);
                    return View("Form", PopluteBookFormViewModel(model));
                }
                var ImageName = $"{model.Title}{extension}";
                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", ImageName);
                using var stream = System.IO.File.Create(path);
                model.Image.CopyTo(stream);
                books.ImageUrl= ImageName;

            }
            foreach (var item in model.SelectedCatgegories)
            {
				books.Catgegories.Add(new BookCategory { CategoryId = item });
            }
            _context.Add(books);
			_context.SaveChanges();
			return RedirectToAction("Index");
         
        }
        public IActionResult Edit(int id)
        {

            var books = _context.Books.Include(x=>x.Catgegories).SingleOrDefault(x=>x.Id==id);
            if (books is null)
                return NotFound();

            var Model = _mapper.Map<BookFormViewModel>(books);
            var ViewModel = PopluteBookFormViewModel(Model);
            ViewModel.SelectedCatgegories=books.Catgegories.Select(c=>c.CategoryId).ToList();
            return View("Form", ViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookFormViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model = PopluteBookFormViewModel(model);
                return View("Form", model);
            }
            var books = _context.Books.Include(x => x.Catgegories).SingleOrDefault(x => x.Id == model.Id);
            if (books is null)
                return NotFound();

            if (model.Image is not null)
            {

                if (!string.IsNullOrEmpty(model.ImageUrl))
                {
                    var oldImage=Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books/", books.ImageUrl!);
                    if (System.IO.File.Exists(oldImage))
                        System.IO.File.Delete(oldImage);
                    
                }
                var extension = Path.GetExtension(model.Image.FileName);

                if (!_allowedExtentions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Error.NotAllowedExtention);
                    return View("Form", PopluteBookFormViewModel(model));
                }
                if (model.Image.Length > _maxSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Error.NotAllowedSize);
                    return View("Form", PopluteBookFormViewModel(model));
                }
                var ImageName = $"{model.Title}{extension}";
                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", ImageName);
                using var stream = System.IO.File.Create(path);
                model.Image.CopyTo(stream);
                model.ImageUrl = ImageName;

            }
            else if(model.Image is null && !string.IsNullOrEmpty(model.ImageUrl))
            {
                model.ImageUrl = books.ImageUrl;
            }

            books = _mapper.Map(model,books);
            books.LastUpdatedOn = DateTime.Now;
            foreach (var item in model.SelectedCatgegories)
            {
                books.Catgegories.Add(new BookCategory { CategoryId = item });
            }
            _context.Update(books);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }


    }
}
