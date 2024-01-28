using Bookify.Core.Consts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Core.ViewModel
{
	public class BookFormViewModel
	{
		public int Id { get; set; }
		[MaxLength(500,ErrorMessage =Error.MaxLength)]
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		[MaxLength(200, ErrorMessage = Error.MaxLength)]
		public string Publisher { get; set; } = null!;
		[Display(Name = "Publishing date")]

		public DateTime PublishingDate { get; set; }= DateTime.Now;
        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }

        public IFormFile? Image { get; set; }
		[MaxLength(50, ErrorMessage = Error.MaxLength)]

		public string Hall { get; set; } = null!;
		[Display(Name = "Is Available For Rental ?")]

		public bool IsAvailableForRental { get; set; }

		//Author
		[Display(Name = "Author")]
		public int AuthorId { get; set; }
		public IEnumerable<SelectListItem>? Authors { get; set; }
        [Display(Name = "Catgegories")]

        public IList<int> SelectedCatgegories { get; set; } = new List<int>();
		public IEnumerable<SelectListItem>? Catgegories { get; set; }

	}
}
