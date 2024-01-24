namespace Bookify.Core.Models
{
    [Index(nameof(Name), IsUnique = true)]

    public class Author : BaseModel
    {
        public int id { get; set; }
        [MaxLength(1000)]
        public string Name { get; set; } = null!;


    }
}
