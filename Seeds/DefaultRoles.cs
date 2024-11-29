using Microsoft.AspNetCore.Identity;

public static class DefaultRoles {
    public static async Task SeedAsync(RoleManager<IdentityRole> _roleManager) {
        
        if (await _roleManager.FindByNameAsync(Roles.SuperAdmin.ToString()) == null) {
            await _roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await _roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
        }
    }
}