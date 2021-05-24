using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Models
{
	public class LibraryContext : IdentityDbContext<ApplicationUser>
	{
		public virtual DbSet<Author> Authors { get; set; }
		public virtual DbSet<Book> Books { get; set; }
		public virtual DbSet<AuthorBook> AuthorBooks { get; set; }

		public LibraryContext(DbContextOptions options) : base(options) { }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
		}
	}
}