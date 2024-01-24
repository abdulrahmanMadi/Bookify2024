namespace Bookify.Core.Models
{
    [Index(nameof(Title),nameof(AuthorId),IsUnique =true)]
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public DateTime PublishingDate { get; set; }
        public string? ImageUrl { get; set; }
        public string Hall { get; set; } = null!;
        public bool IsAvailableForRental { get; set; }

        //Author
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        public ICollection<BookCategory> Catgegories { get; set;} = new List<BookCategory>();




    }
}
