using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GFS.Data.EFCore
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GfsDbContext>
    {
        public GfsDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<GfsDbContext>();
            const string connectionString =
                "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=inzynierka-gfs;";
            const string connectionString2 =
                "User ID=wjrqsenc;Password=CzGu3L52DXhGO3jl065i-Xzpv8e-o2at;Host=dumbo.db.elephantsql.com;Port=5432;Database=wjrqsenc;";
            builder.UseNpgsql(connectionString2);
            return new GfsDbContext(builder.Options);
        }

    }
}