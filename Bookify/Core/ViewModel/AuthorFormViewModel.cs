
using Bookify.Core.Consts;

namespace Bookify.Core.ViewModel
{
    public class AuthorFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(150, ErrorMessage = Error.MaxLength), Display(Name = "Category")]
        [Remote("AllowItem", null!, AdditionalFields = "Id", ErrorMessage = Error.Duplicated)]
        public string Name { get; set; } = null!;
    }
}
