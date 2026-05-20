using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Infrastructure.Identity;

public static class UserSeeder
{
    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager)
    {
        await SeedAdmin(userManager);
        await SeedManager(userManager);
        await SeedUser(userManager);
    }

    private static async Task SeedAdmin(
        UserManager<ApplicationUser> userManager)
    {
        var email = "admin@taskmanagement.com";

        if (await userManager.FindByEmailAsync(email) is not null)
            return;

        var user = new ApplicationUser
        {
            FullName = "System Admin",
            Email = email,
            UserName = email
        };

        await userManager.CreateAsync(user, "Admin123!");

        await userManager.AddToRoleAsync(user, "Admin");
    }

    private static async Task SeedManager(
        UserManager<ApplicationUser> userManager)
    {
        var email = "manager@taskmanagement.com";

        if (await userManager.FindByEmailAsync(email) is not null)
            return;

        var user = new ApplicationUser
        {
            FullName = "Project Manager",
            Email = email,
            UserName = email
        };

        await userManager.CreateAsync(user, "Manager123!");

        await userManager.AddToRoleAsync(user, "Manager");
    }

    private static async Task SeedUser(
        UserManager<ApplicationUser> userManager)
    {
        var email = "user@taskmanagement.com";

        if (await userManager.FindByEmailAsync(email) is not null)
            return;

        var user = new ApplicationUser
        {
            FullName = "Normal User",
            Email = email,
            UserName = email
        };

        await userManager.CreateAsync(user, "User123!");

        await userManager.AddToRoleAsync(user, "User");
    }
}