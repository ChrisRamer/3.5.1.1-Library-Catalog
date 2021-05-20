using Microsoft.EntityFrameworkCore;

namespace Library.Models
{
	public class LibraryContext : DbContext
	{
		//public virtual DbSet<Library> XXX_Model1Name { get; set; }
		//public virtual DbSet<Library> XXX_Model2Name { get; set; }

		public LibraryContext(DbContextOptions options) : base(options) { }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
		}
	}
}