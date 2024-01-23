using Microsoft.EntityFrameworkCore;

namespace Bookify.Core.Models
{
	[Index(nameof(Name),IsUnique =true)]
	public class Category
	{
        public int id { get; set; }
		[MaxLength(1000)]
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }
		public DateTime CreatedOn { get; set; }= DateTime.Now;
		public DateTime? LastUpdatedOn { get; set; } 


	}
}
