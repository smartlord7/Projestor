using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace AspNetIdentity.WebApi.Infrastructure
{
    public class AppDbContext : IdentityDbContext<User>
    {

        public DbSet<Issue> Issues { get; set; }

        public DbSet<Project> Projects { get; set; }
        public AppDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static AppDbContext Create() {
            return new AppDbContext();
        }

    }
}
