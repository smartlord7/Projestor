using AspNetIdentity.WebApi.Validators;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;

namespace AspNetIdentity.WebApi.Infrastructure
{
     public class AppUserManager : UserManager<User>
    {

        public AppUserManager(IUserStore<User> store) : base(store)
        {

        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> opti, IOwinContext ctx)
        {
            var appDbContext = ctx.Get<AppDbContext>();
            var appUserManager = new AppUserManager(new UserStore<User>(appDbContext));

            appUserManager.EmailService = new Services.EmailService();

            var dataProtectionProvider = opti.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }

            appUserManager.UserValidator = new CustomUserValidator(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
            };


            return appUserManager;
        }
    }
}