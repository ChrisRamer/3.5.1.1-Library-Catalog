using System.Collections.Generic;

namespace Library.Models
{
	public class Book
	{
		public int BookId { get; set; }
		public string Name { get; set; }
		public virtual ApplicationUser User { get; set; }
		public virtual ICollection<AuthorBook> Authors { get; }

		public Book()
		{
			this.Authors = new HashSet<AuthorBook>();
		}
	}
}