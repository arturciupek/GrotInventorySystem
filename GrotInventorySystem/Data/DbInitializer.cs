using Microsoft.AspNetCore.Identity;

namespace GrotInventorySystem.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services, string adminEmail, string adminPassword, string operatorEmail, string operatorPassword, string serwisEmail, string serwisPassword, string odczytEmail, string odczytPassword)
    {
        using var scope = services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Roles
        string[] roles = { "Admin", "Operator", "Serwis", "Odczyt" };
        foreach (var r in roles)
            if (!await roleManager.RoleExistsAsync(r))
                await roleManager.CreateAsync(new IdentityRole(r));

        // Admin
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            await userManager.CreateAsync(admin, adminPassword);
        }

        if (!await userManager.IsInRoleAsync(admin, "Admin"))
            await userManager.AddToRoleAsync(admin, "Admin");

        // Operator
        var operatorUser = await userManager.FindByEmailAsync(operatorEmail);
        if (operatorUser is null)
        {
            operatorUser = new ApplicationUser { UserName = operatorEmail, Email = operatorEmail, EmailConfirmed = true };
            await userManager.CreateAsync(operatorUser, operatorPassword);
        }

        if (!await userManager.IsInRoleAsync(operatorUser, "Operator"))
            await userManager.AddToRoleAsync(operatorUser, "Operator");

        // Serwis
        var serwis = await userManager.FindByEmailAsync(serwisEmail);
        if (serwis is null)
        {
            serwis = new ApplicationUser { UserName = serwisEmail, Email = serwisEmail, EmailConfirmed = true };
            await userManager.CreateAsync(serwis, serwisPassword);
        }

        if (!await userManager.IsInRoleAsync(serwis, "Serwis"))
            await userManager.AddToRoleAsync(serwis, "Serwis");

        // Odczyt
        var odczyt = await userManager.FindByEmailAsync(odczytEmail);
        if (odczyt is null)
        {
            odczyt = new ApplicationUser { UserName = odczytEmail, Email = odczytEmail, EmailConfirmed = true };
            await userManager.CreateAsync(odczyt, odczytPassword);
        }

        if (!await userManager.IsInRoleAsync(odczyt, "Odczyt"))
            await userManager.AddToRoleAsync(odczyt, "Odczyt");


    }
}