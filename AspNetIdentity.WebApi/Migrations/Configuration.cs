namespace AspNetIdentity.WebApi.Migrations
{ 

    using AspNetIdentity.WebApi.Infrastructure;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AspNetIdentity.WebApi.Infrastructure.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AspNetIdentity.WebApi.Infrastructure.AppDbContext context)
        {
            var manager = new UserManager<User>(new UserStore<User>(new AppDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AppDbContext()));

        var user = new User()
        {
            UserName = "Smartlord",
            Email = "smartlord7@gmail.com",
            EmailConfirmed = true,
            FirstName = "Smart",
            LastName = "Lord",
            Bio = "Most powerful user of Projestor",
            BirthDate = new DateTime(2001, 6, 23),
            Job = "Programmer",
            PhoneNumber = "967116687",
            Gend = User.Gender.MALE
            };

            manager.Create(user, "familinha123");

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "Manager" });
                roleManager.Create(new IdentityRole { Name = "Programmer" });
            }

            var adminUser = manager.FindByName("Smartlord");

            manager.AddToRoles(adminUser.Id, new string[] { "Admin", "Manager" });
        }
    }
}
