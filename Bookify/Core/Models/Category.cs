﻿
namespace Bookify.Core.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category : BaseModel
    {
        public int id { get; set; }
        [MaxLength(1000)]
        public string Name { get; set; } = null!;

        public ICollection<BookCategory> Books { get; set; } = new List<BookCategory>();
    }
}
