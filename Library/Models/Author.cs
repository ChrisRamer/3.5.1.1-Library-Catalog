using System.Collections.Generic;

namespace Library.Models
{
	public class Author
	{
		public int AuthorId { get; set; }
		public string Name { get; set; }
		public virtual ApplicationUser User { get; set; }
		public virtual ICollection<AuthorBook> Books { get; set; }

		public Author()
		{
			this.Books = new HashSet<AuthorBook>();
		}
	}
}