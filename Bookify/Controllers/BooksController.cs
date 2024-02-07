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
		private List<string> _allowedExtentions = new (){ ".jpg",".png", ".jpeg" , ".JPG", ".PNG", ".JPEG" };
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
        [AjaxOnly]
        public IActionResult GetBooks()
        {
            IQueryable<Book> Books = _context.Books;
            var data= Books.Skip(0).Take(10).ToList();
            var recoedsTotal=Books.Count();
            var jsonData = new { recoedFilter = recoedsTotal, recoedsTotal, data };
            return View(jsonData);
        }
        public IActionResult Details(int id)
        {
            var book = _context.Books
                .Include(x => x.Author)
				.Include(x => x.Catgegories)
                .ThenInclude(x=>x.Category)
				.SingleOrDefault(x => x.Id == id);
            if (book == null)
                return NotFound();
            var viewmodel = _mapper.Map<BookViewModel>(book);
            return View(viewmodel);
        }
        public IActionResult Create()
		{
			var viewModel = PopluteBookFormViewModel();
            return View("Form", viewModel);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookFormViewModel model)
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
                var pathThumb = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books/Thumb", ImageName);

                using var stream = System.IO.File.Create(path);
                await model.Image.CopyToAsync(stream);
                stream.Dispose();
                books.ImageThumbUrl = $"/Images/Books/Thumb/{ImageName}";
                books.ImageUrl = $"/Images/Books/{ImageName}";

                using var image = Image.Load(model.Image.OpenReadStream());
                var ratio = (float)image.Width / 200;
                var heigh = image.Height / ratio;
                image.Mutate(i => i.Resize(200, (int)heigh));
                image.Save(pathThumb);

            }
            foreach (var item in model.SelectedCatgegories)
            {
				books.Catgegories.Add(new BookCategory { CategoryId = item });
            }
            _context.Add(books);
			_context.SaveChanges();
			return RedirectToAction(nameof(Details), new {id=books.Id});
         
        }
        public  IActionResult Edit(int id)
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
        public async Task<IActionResult> Edit(BookFormViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View("Form", PopluteBookFormViewModel(model));
            }
            var books = _context.Books.Include(x => x.Catgegories).SingleOrDefault(x => x.Id == model.Id);
            if (books is null)
                return NotFound();

            if (model.Image is not null)
            {
                if (!string.IsNullOrEmpty(model.ImageUrl))
                {
                    var oldImage=       $"{_webHostEnvironment.WebRootPath}{model.ImageUrl}";
                    var oldThumbImage = $"{_webHostEnvironment.WebRootPath}{model.ImageThumbUrl}";

                    if (System.IO.File.Exists(oldImage))
                        System.IO.File.Delete(oldImage);
                    if (System.IO.File.Exists(oldThumbImage))
                        System.IO.File.Delete(oldThumbImage);
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
				var pathThumb = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books/Thumb", ImageName);

				using var stream = System.IO.File.Create(path);
                await model.Image.CopyToAsync(stream);
                stream.Dispose();
                model.ImageThumbUrl= $"/Images/Books/Thumb/{ImageName}";
				model.ImageUrl =$"/Images/Books/{ImageName}";

                using var image = Image.Load(model.Image.OpenReadStream());
                var ratio = (float)image.Width / 200;
                var heigh = image.Height / ratio;
                image.Mutate(i => i.Resize(200, (int)heigh));
                image.Save(pathThumb);
            }
            else if(!string.IsNullOrEmpty(books.ImageUrl))
            {
                model.ImageUrl = books.ImageUrl;
                model.ImageThumbUrl = books.ImageThumbUrl;
            } 

            books = _mapper.Map(model,books);
            books.LastUpdatedOn = DateTime.Now;
            foreach (var item in model.SelectedCatgegories)
            {
                books.Catgegories.Add(new BookCategory { CategoryId = item });
            }
            _context.Update(books);
            _context.SaveChanges();
            return RedirectToAction(nameof(Details), new { id = books.Id });

        }
        public IActionResult AllowItem(BookFormViewModel model)
        {
            var IsExist = _context.Books.SingleOrDefault(x => x.Title == model.Title && x.AuthorId == model.AuthorId);
            var isAllowed = IsExist is null || IsExist.Id.Equals(model.Id);
            return Json(isAllowed);
        }


    }
}
