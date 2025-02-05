using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.API.Constants;
using UserManagement.API.Models;

namespace UserManagement.API.Data
{
    public class DbSeeder
    {
        public static async Task SeedData(IApplicationBuilder app)
        {
            // Create a scoped service provider to resolve dependencies
            using var scope = app.ApplicationServices.CreateScope();
            // resolve the logger service
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbSeeder>>();

            try
            {
                // resolve other dependencies
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // 🚀 Avoid redundant seeding
                if (await userManager.Users.AnyAsync() && await roleManager.Roles.AnyAsync())
                {
                    logger.LogInformation("Skipping DB Seeding: Users and roles already exist.");
                    return;
                }

                // ✅ Create roles only if they don’t exist
                await EnsureRoleExists(roleManager, Roles.Admin, logger);
                await EnsureRoleExists(roleManager, Roles.User, logger);

                // ✅ Ensure admin user exists
                var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        Name = "Admin",
                        UserName = "admin@gmail.com",
                        Email = "admin@gmail.com",
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var createUserResult = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (!createUserResult.Succeeded)
                    {
                        logger.LogError($"Failed to create Admin user: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
                        return;
                    }
                }

                // ✅ Ensure admin has the role
                if (!await userManager.IsInRoleAsync(adminUser, Roles.Admin))
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(adminUser, Roles.Admin);
                    if (!addToRoleResult.Succeeded)
                    {
                        logger.LogError($"Failed to assign Admin role: {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
                    }
                }

                logger.LogInformation("✅ DB Seeding Completed Successfully");
            }

            catch (Exception ex)
            {
                logger.LogCritical(ex.Message);
            }
        }

        private static async Task EnsureRoleExists(RoleManager<IdentityRole> roleManager, string roleName, ILogger logger)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                logger.LogInformation($"Creating role: {roleName}");
                var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                {
                    logger.LogError($"Failed to create {roleName} role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }
                else
                {
                    logger.LogInformation($"✅ Role Created: {roleName}");
                }
            }
        }
    }
}
