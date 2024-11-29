namespace PizzaLab.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Web.ViewModels.Pizza;

    using static PizzaLab.Common.NotificationMessagesConstants;

    public class PizzaController : BaseController
    {
        private readonly IPizzaService pizzaService;
        private readonly IProductService productService;
        private readonly IDoughService doughService;


        public PizzaController(
            IPizzaService _pizzaService, 
            IProductService _productService,
            IDoughService _doughService
            )
        {
            this.pizzaService = _pizzaService;
            this.productService = _productService;
            this.doughService = _doughService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<PizzasForMenuViewModel> model = await pizzaService
                .GetAllPizzasAsync();

            return View(model);
        }
       
        [HttpGet]
        public async Task<IActionResult> Details(int pizzaId, string returnUrl)
        {
            try
            {
                PizzaDetailsViewModel? model = await pizzaService.GetPizzaByIdAsync(pizzaId);
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Unexpected error occured. Try later or contact administrator!";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> OrderPizza(int pizzaId, string returnUrl)
        {
            try
            {
                OrderPizzaViewModel? model = await pizzaService.GetPizzaForOrderAsync(pizzaId);
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Unexpected error occured. Try later or contact administrator!";
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
