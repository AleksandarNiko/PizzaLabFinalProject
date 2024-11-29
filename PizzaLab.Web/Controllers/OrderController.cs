namespace PizzaLab.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using PizzaLab.Services.Data.Interfaces;

    using static PizzaLab.Common.NotificationMessagesConstants;

    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService _orderService)
        {
            this.orderService = _orderService;
        }

        public IActionResult ConfirmOrder()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MakeOrder()
        {
            string userId = GetUserId();

            try
            {
                await orderService.AddOrderAsync(userId);
                await orderService.RemoveCartPizzasAsync(userId);
                await orderService.EmptyCartAsync(userId);

                TempData[SuccessMessage] = "Your Order Was Successful!";

                return RedirectToAction("All", "Menu");
            }
            catch
            {
                TempData[ErrorMessage] = "Unexpected error occured. Try later or contact administrator!";
                return RedirectToAction("FinalizeOrder", "Cart");
            }
            
        }
    }
}
