using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace MatteoFabbri.EntityFrameworkCore
{
    public static class MatteoFabbriDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<MatteoFabbriDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<MatteoFabbriDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
