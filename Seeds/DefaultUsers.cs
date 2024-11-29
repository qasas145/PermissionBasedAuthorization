using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

public static class DefaultUsers {
    public static async Task SeedBasicAsync(UserManager<IdentityUser> _userManager) {

        if (await _userManager.FindByEmailAsync("basic@gmail.com") == null) {

            var user01 = new IdentityUser() {
                Email = "basic@gmail.com",
                UserName = "basic"
            };

            await _userManager.CreateAsync(user01, "Hamada1020$");
            await _userManager.AddToRoleAsync(user01, Roles.Basic.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("the basic has been created");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    public static async Task SeedSuperAdmin(UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager) {
        if (await _userManager.FindByEmailAsync("superadmin@yahoo.com") == null){
            var user01 = new IdentityUser() {
                Email = "superadmin@yahoo.com",
                UserName = "superadmin"
            };
            await _userManager.CreateAsync(user01, "Hamada1020$");
            await _userManager.AddToRolesAsync(user01, new List<string>(){Roles.SuperAdmin.ToString(), Roles.Admin.ToString(), Roles.Basic.ToString()});
            
            await _roleManager.SeedClaimsForSuperUser();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("the superadmin has been created");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    private static async Task SeedClaimsForSuperUser(this RoleManager<IdentityRole> _roleManager) {

        var role = await _roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
        await _roleManager.AddPermissionsClaims(role, "Products");

    }
    private static async Task AddPermissionsClaims(this RoleManager<IdentityRole> _roleManager,IdentityRole role, string module){
        var allClaims = await _roleManager.GetClaimsAsync(role);
        var permissions = Permissions.GeneratePermissions(module);
        foreach(var per in permissions) {
            if (!allClaims.Any(c=>c.Type == "Permission" && c.Value == per)){
                await _roleManager.AddClaimAsync(role, new Claim("Permission", per));
            }
        }

    }
}