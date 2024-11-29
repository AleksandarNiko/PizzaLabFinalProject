namespace PizzaLab.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Web.ViewModels.Menu;

    public class MenuController : BaseController
    {
        private readonly IMenuService menuService;
        private readonly IPizzaService pizzaService;

        public MenuController(IMenuService _menuService, IPizzaService _pizzaService)
        {
            menuService = _menuService;
            pizzaService = _pizzaService;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<MenuViewModel> model = await menuService.GetAllMenusAsync();

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Pizzas(int menuId)
        {
            try
            {
                var pizzas = await pizzaService.GetPizzasByMenuIdAsync(menuId);
                ViewBag.MenuId = menuId;
                return View(pizzas);
            }
            catch
            {
                TempData["ErrorMessage"] = "Something went wrong";
                return RedirectToAction("All", "Menu");
            }

        }
    }
}
