namespace Bookify.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var Authors = _context.Authors.AsNoTracking().ToList();
            var viewModel = _mapper.Map<IEnumerable<AuthorViewModel>>(Authors);
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
        public IActionResult Create(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var author = _mapper.Map<Author>(model);


            var viewModel = _mapper.Map<AuthorViewModel>(author);

            _context.Add(author);
            _context.SaveChanges();
            TempData["Message"] = "Item Created Successfuly!";
            return RedirectToAction("Index", viewModel);
        }
        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var author = _context.Authors.Find(id);
            if (author == null)
                return NotFound();
            //var authorModel = new AuthorFormViewModel
            //{
            //    Id = id,
            //    Name = author.Name,
            //};
            var authorModel = _mapper.Map<AuthorFormViewModel>(author);

            return PartialView("_Form", authorModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var author = _context.Authors.Find(model.Id);
            if (author == null)
                return NotFound();
            //   author.Name = model.Name;
            author = _mapper.Map(model, author);

            author.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();
            TempData["Message"] = "Item Updated Successfuly!";
            var viewModel = _mapper.Map<AuthorViewModel>(author);

            return RedirectToAction("Index", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult ToggleStatus(int id)
        {
            var author = _context.Authors.Find(id);
            if (author == null)
                return NotFound();
            author.IsDeleted = !author.IsDeleted;
            author.LastUpdatedOn = DateTime.Now;

            _context.SaveChanges();
            return Ok(author.LastUpdatedOn.ToString());
        }
        public IActionResult AllowItem(AuthorFormViewModel model)
        {
            var IsExist = _context.Authors.SingleOrDefault(x => x.Name == model.Name);
            var isAllowed = IsExist is null || IsExist.id.Equals(model.Id);
            return Json(isAllowed);
        }
    }
}
