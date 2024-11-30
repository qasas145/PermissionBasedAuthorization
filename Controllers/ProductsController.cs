using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public  class ProductsController : Controller {
    // [Permission("Permission.Products.Create")]
    [HttpGet]
    [Authorize("Permission.Products.Create")]
    public async Task<IActionResult> TestApi() {
        Console.WriteLine("we are in the controller");
        return View();
    }
    [HttpGet]
    [Authorize("Permission.Products.Edit")]
    public async Task<IActionResult> TestEdit() {
        return View();
    }
}