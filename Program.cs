using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(f=>f.Filters.Add<PermissionFilter>());

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EmailSender>();

builder.Services.AddRazorPages();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PolicyHandler>();
builder.Services.Configure<SecurityStampValidatorOptions>(options=>{
    options.ValidationInterval = TimeSpan.Zero;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// seeding roles and users
using (var scope = app.Services.CreateScope()){
    
    var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerProvider>();
    var logger = loggerFactory.CreateLogger("app");
    logger.LogInformation("Hello saye elqasas");

    try {
        
        var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        DefaultRoles.SeedAsync(_roleManager).GetAwaiter().GetResult();
        DefaultUsers.SeedBasicAsync(_userManager).GetAwaiter().GetResult();
        DefaultUsers.SeedSuperAdmin(_userManager, _roleManager).GetAwaiter().GetResult();

        logger.LogInformation("The data has been seeded");
        logger.LogInformation("application started");

    }catch(Exception e) {
        logger.LogWarning(e, "An error has happened");
    }

}
app.MapRazorPages();
app.Run();
