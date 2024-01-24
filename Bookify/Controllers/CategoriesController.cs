

namespace Bookify.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public CategoriesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var categories = _context.Categories.AsNoTracking().ToList();
            var viewModel = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
            return View(viewModel);
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
            if (!ModelState.IsValid)
                return View(model);
            var category = _mapper.Map<Category>(model);


            var viewModel = _mapper.Map<CategoryViewModel>(category);

            _context.Add(category);
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
            //var CategoryModel = new CategoryFormViewModel
            //{
            //    Id = id,
            //    Name = category.Name,
            //};
            var CategoryModel = _mapper.Map<CategoryFormViewModel>(category);

            return PartialView("_Form", CategoryModel);
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
            //   category.Name = model.Name;
            category = _mapper.Map(model, category);

            category.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();
            TempData["Message"] = "Item Updated Successfuly!";
            var viewModel = _mapper.Map<CategoryViewModel>(category);

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
            category.LastUpdatedOn = DateTime.Now;

            _context.SaveChanges();
            return Ok(category.LastUpdatedOn.ToString());
        }
        public IActionResult AllowItem(CategoryFormViewModel model)
        {
            var IsExist = _context.Categories.SingleOrDefault(x => x.Name == model.Name);
            var isAllowed = IsExist is null || IsExist.id.Equals(model.Id);
            return Json(isAllowed);
        }
    }
}
