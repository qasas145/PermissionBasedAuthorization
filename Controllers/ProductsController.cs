using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public  class ProductsController : Controller {
    [HttpGet]
    [Authorize("Permission.Products.Create")]
    public async Task<IActionResult> TestApi() {
        return View();
    }
    [HttpGet]
    [Authorize("Permission.Products.Edit")]
    public async Task<IActionResult> TestEdit() {
        return View();
    }
}