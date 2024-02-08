using Bookify.Core.Consts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Core.ViewModel
{
	public class BookFormViewModel
	{
		public int Id { get; set; }
		[MaxLength(500,ErrorMessage =Error.MaxLength)]
        [Remote("AllowItem", null!, AdditionalFields = "Id,AuthorId", ErrorMessage = Error.DuplicatedBooks)]
        public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		[MaxLength(200, ErrorMessage = Error.MaxLength)]
		public string Publisher { get; set; } = null!;
		[Display(Name = "Publishing date")]

		public DateTime PublishingDate { get; set; }= DateTime.Now;
        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }
        public string? ImageThumbUrl { get; set; }

        public IFormFile? Image { get; set; }
		[MaxLength(50, ErrorMessage = Error.MaxLength)]

		public string Hall { get; set; } = null!;
		[Display(Name = "Is Available For Rental ?")]

		public bool IsAvailableForRental { get; set; }


		//Author
		[Display(Name = "Author")]
        [Remote("AllowItem", null!, AdditionalFields = "Id,Title", ErrorMessage = Error.DuplicatedBooks)]
        public int AuthorId { get; set; }
		public IEnumerable<SelectListItem>? Authors { get; set; }


        //Catgegories
        [Display(Name = "Catgegories")]
        public IList<int> SelectedCatgegories { get; set; } = new List<int>();
		public IEnumerable<SelectListItem>? Categories { get; set; }

	}
}
