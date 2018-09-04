using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using MatteoFabbri.Authorization.Roles;
using MatteoFabbri.Authorization.Users;
using MatteoFabbri.MultiTenancy;

namespace MatteoFabbri.EntityFrameworkCore
{
    public class MatteoFabbriDbContext : AbpZeroDbContext<Tenant, Role, User, MatteoFabbriDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public MatteoFabbriDbContext(DbContextOptions<MatteoFabbriDbContext> options)
            : base(options)
        {
        }
    }
}
