namespace Bookify.Core.ViewModel
{
    public class CategoryViewModel
    {
        public int id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } 
        public DateTime? LastUpdatedOn { get; set; }
    }
}
