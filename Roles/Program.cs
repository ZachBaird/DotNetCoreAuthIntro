using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Roles.Data;
using System.Linq;

namespace Roles
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create web host builder.
            var host = CreateWebHostBuilder(args).Build();

            using (var services = host.Services.CreateScope())
            {
                var dbContext = services.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userMgr = services.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleMgr = services.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                dbContext.Database.Migrate();

                var adminRole = new IdentityRole("Admin");

                // If there's no roles in the DB, create an admin role.
                if (!dbContext.Roles.Any())
                    roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();

                // If there are no users with the name of "admin", create an IdentityUser.
                if (!dbContext.Users.Any(u => u.UserName == "admin"))
                {
                    var adminUser = new IdentityUser
                    {
                        UserName = "admin@test.com",
                        Email = "admin@test.com"
                    };

                    // Create the user with a password.
                    var result = userMgr.CreateAsync(adminUser, "password").GetAwaiter().GetResult();

                    // Add the user to the admin role.
                    userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
                }
            }

            // Run web host.
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
