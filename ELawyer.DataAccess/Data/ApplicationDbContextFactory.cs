using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ELawyer.DataAccess.Data
{
  public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
  {
    public ApplicationDbContext CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

      var configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .Build();

      var connectionString = configuration.GetConnectionString("Legal");

      optionsBuilder.UseSqlServer(connectionString);

      return new ApplicationDbContext(optionsBuilder.Options);
    }
  }
}
