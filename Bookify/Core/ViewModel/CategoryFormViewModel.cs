using Microsoft.AspNetCore.Mvc;

namespace Bookify.Core.ViewModel
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }
        [Remote("AllowItem",null,AdditionalFields ="Id",ErrorMessage ="This Category is already exists !")]
        public string Name { get; set; } = null!;
    }
}
