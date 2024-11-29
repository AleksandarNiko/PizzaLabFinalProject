namespace PizzaLab.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Web.ViewModels.Cart;

    using static PizzaLab.Common.NotificationMessagesConstants;
    public class CartController : BaseController
    {
        private readonly ICartService cartService;

        public CartController(ICartService _cartService)
        {
            this.cartService = _cartService;
        }

        [HttpGet]
        public IActionResult OrderConfirmation()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> FinalizeOrder()
        {
            try
            {
                string userId = GetUserId();
                List<CartItemViewModel> cartItems = await cartService.GetAllCartItemsAsync(userId);
                decimal finalPrize = await cartService.GetFinalPrizeAsync(userId);

                ViewCartViewModel model = new ViewCartViewModel()
                {
                    Items = cartItems,
                    FinalPrize = finalPrize
                };

                return View(model);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Unexpected error occured. Try later or contact administrator!";
                return RedirectToAction("All", "Menu");
            }

        }
    

        [HttpPost]
        public async Task<IActionResult> AddToCart(int pizzaId, decimal updatedTotalPrice)
        {
            try
            {
                string userId = GetUserId();
                await cartService.AddPizzaToCartAsync(pizzaId, updatedTotalPrice, userId);
                TempData[SuccessMessage] = "Pizza was successfuly added to the cart";

                return RedirectToAction("OrderConfirmation", "Cart");

            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, "Unexpected error occurred! Please try again later or contact administrator!");

                return RedirectToAction("All", "Pizza");
            }
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFromCart(int cartId, int pizzaId)
        {
            try
            {
                string userId = GetUserId();
                await cartService.RemovePizzaFromCartAsync(cartId, pizzaId, userId);

                TempData[SuccessMessage] = "Pizza was successfully removed from the cart";

                List<CartItemViewModel> cartItems = await cartService.GetAllCartItemsAsync(userId);
                decimal finalPrize = await cartService.GetFinalPrizeAsync(userId);

                ViewCartViewModel model = new ViewCartViewModel()
                {
                    Items = cartItems,
                    FinalPrize = finalPrize
                };

                return View("FinalizeOrder", model);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, "Unexpected error occurred! Please try again later or contact administrator!");
                string userId = GetUserId();

                List<CartItemViewModel> cartItems = await cartService.GetAllCartItemsAsync(userId);
                decimal finalPrize = await cartService.GetFinalPrizeAsync(userId);

                ViewCartViewModel model = new ViewCartViewModel()
                {
                    Items = cartItems,
                    FinalPrize = finalPrize
                };

                return View("FinalizeOrder", model);
            }
        }
    }
}
