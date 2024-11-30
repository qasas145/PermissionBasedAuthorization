using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class UserController : Controller {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    [HttpGet]
    public async Task<IActionResult> Index() {
        var users = await _userManager.Users.ToListAsync();
        var usersDTO = users.Select(u=>new UserDTO(){
            Email = u.Email, 
            UserName = u.UserName, 
            Roles = _userManager.GetRolesAsync(u).GetAwaiter().GetResult()
        });
        return View(usersDTO);
    }
    [HttpGet]
    public async Task<IActionResult> ManageRoles(string userName) {
        var user = await _userManager.FindByNameAsync(userName);

        var roles = await _roleManager.Roles.ToListAsync();


        var rolesViewModel = new UserRoleViewModel() {
            UserName =userName,
            Roles = roles.Select(r=>new CheckBoxDTO(){
                DisplayName = r.Name, 
                IsSelected= _userManager.IsInRoleAsync(user,r.Name).GetAwaiter().GetResult()
            }).ToList(),
        };
        return View(rolesViewModel);
    }
    [HttpPost]
    public async Task<IActionResult> ManageRoles([FromForm]UserRoleViewModel model) {

        if (!ModelState.IsValid) 
            return View(ModelState);
        
        var selectedRoles = model.Roles.Where(r=>r.IsSelected == true).Select(r=>r.DisplayName);
        
        var user = await _userManager.FindByNameAsync(model.UserName);
        var userRoles = await _userManager.GetRolesAsync(user);


        await _userManager.RemoveFromRolesAsync(user,userRoles);

        await _userManager.AddToRolesAsync(user,selectedRoles);
        
        return RedirectToAction(nameof(Index));
    }
}