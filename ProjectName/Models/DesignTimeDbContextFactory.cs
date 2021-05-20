using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace XXX_ProjectName.Models
{
	public class XXX_ProjectNameContextFactory : IDesignTimeDbContextFactory<XXX_ProjectNameContext>
	{

		XXX_ProjectNameContext IDesignTimeDbContextFactory<XXX_ProjectNameContext>.CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
			  .SetBasePath(Directory.GetCurrentDirectory())
			  .AddJsonFile("appsettings.json")
			  .Build();

			var builder = new DbContextOptionsBuilder<XXX_ProjectNameContext>();
			string connectionString = configuration.GetConnectionString("DefaultConnection");

			builder.UseMySql(connectionString);

			return new XXX_ProjectNameContext(builder.Options);
		}
	}
}