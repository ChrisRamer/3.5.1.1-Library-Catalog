using Microsoft.EntityFrameworkCore;

namespace XXX_ProjectName.XXX_Models
{
	public class XXX_ProjectNameContext : DbContext
	{
		//public virtual DbSet<XXX_ProjectName> XXX_Model1Name { get; set; }
		//public virtual DbSet<XXX_ProjectName> XXX_Model2Name { get; set; }

		public XXX_ProjectNameContext(DbContextOptions options) : base(options) { }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
		}
	}
}