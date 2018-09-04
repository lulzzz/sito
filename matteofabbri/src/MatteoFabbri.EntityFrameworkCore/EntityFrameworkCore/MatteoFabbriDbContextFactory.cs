using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MatteoFabbri.Configuration;
using MatteoFabbri.Web;

namespace MatteoFabbri.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class MatteoFabbriDbContextFactory : IDesignTimeDbContextFactory<MatteoFabbriDbContext>
    {
        public MatteoFabbriDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MatteoFabbriDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            MatteoFabbriDbContextConfigurer.Configure(builder, configuration.GetConnectionString(MatteoFabbriConsts.ConnectionStringName));

            return new MatteoFabbriDbContext(builder.Options);
        }
    }
}
