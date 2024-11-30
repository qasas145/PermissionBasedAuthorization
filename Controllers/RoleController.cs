using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[Authorize]
public class RoleController : Controller {
    private readonly RoleManager<IdentityRole> _roleManager;
    public RoleController(RoleManager<IdentityRole> roleManager) {
        _roleManager =roleManager;
    }
    public async Task<IActionResult> Index() {
        var roles = await _roleManager.Roles.ToListAsync();
        return View(roles);
    }
    [HttpPost]
    public async Task<IActionResult> AddRole(RoleFormViewModel model) {
        if (!ModelState.IsValid) 
            return View(nameof(Index), _roleManager.Roles.ToListAsync());
        if (await _roleManager.RoleExistsAsync(model.Role)) {
            ModelState.AddModelError("Role", "the role exists already");
            return View(nameof(Index), await _roleManager.Roles.ToListAsync());
        }
        await _roleManager.CreateAsync(new IdentityRole(model.Role));
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> ManagePermissions(string roleId) {
        var role = await _roleManager.FindByIdAsync(roleId);
        var roleClaims = await _roleManager.GetClaimsAsync(role);
        var permissions = Permissions
            .GeneratePermissions("Products")
            .Select(p=>new CheckBoxDTO(){
                DisplayName=p,
                IsSelected = roleClaims.Any(c=>c.Type == "Permission" && c.Value == p)
            });
        var viewModel = new RolePermissionsViewModel(){
            Permissions = permissions.ToList(),
            RoleId = roleId,
            RoleName = role.Name
        };
        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> ManagePermissions(RolePermissionsViewModel model) {
        if (!ModelState.IsValid) 
            return View(model);
        
        var role = await _roleManager.FindByIdAsync(model.RoleId);
        var roleClaims = await _roleManager.GetClaimsAsync(role);
        foreach(var claim in roleClaims) 
            await _roleManager.RemoveClaimAsync(role, claim);

        var selectedClaims = model.Permissions.Where(p=>p.IsSelected == true).ToList();
        foreach (var item in selectedClaims)
            await _roleManager.AddClaimAsync(role, new Claim("Permission", item.DisplayName));
        return RedirectToAction(nameof(ManagePermissions), new {roleId = model.RoleId});
    }

}