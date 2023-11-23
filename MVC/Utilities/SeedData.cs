using Microsoft.AspNetCore.Identity;

namespace MVC.Utilities
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services
            .GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);
            var userManager = services
            .GetRequiredService<UserManager<IdentityUser>>();
            await EnsureAdminAsync(userManager);
        }

        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<string> { UserConstants.AdminRole, UserConstants.UserRole };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }
        private static async Task EnsureAdminAsync(UserManager<IdentityUser> userManager)
        {
            string email = "admin@admin.com";
            string password = "Admin123!";
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser { UserName = email, Email = email };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, UserConstants.AdminRole);
                }
                else
                {
                    throw new Exception(string.Join("\n", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
