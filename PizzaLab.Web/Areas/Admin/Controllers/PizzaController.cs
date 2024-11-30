using Microsoft.AspNetCore.Mvc;
using PizzaLab.Services.Data.Interfaces;
using PizzaLab.Web.ViewModels.Pizza;

using static PizzaLab.Common.NotificationMessagesConstants;

namespace PizzaLab.Web.Areas.Admin.Controllers
{
    public class PizzaController : BaseAdminController
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
        public async Task<IActionResult> Add()
        {
            AddPizzaViewModel model = new AddPizzaViewModel()
            {
                AvailableProducts = await productService.GetAllProductsAsync(),
                Doughs = await doughService.GetAllDoughsAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPizzaViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.AvailableProducts = await productService.GetAllProductsAsync();
                model.Doughs = await doughService.GetAllDoughsAsync();
                TempData[ErrorMessage] = "Something went wrong!";

                return View(model);
            }

            try
            {
                await pizzaService.AddPizzaAsync(model);
                TempData[SuccessMessage] = "Pizza was added successfully!";

                return RedirectToAction("Index", "Home", new { area = "Admin" });

            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, "Unexpected error occurred! Please try again later or contact administrator!");
                model.AvailableProducts = await productService.GetAllProductsAsync();
                model.Doughs = await doughService.GetAllDoughsAsync();

                return this.View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            EditPizzaViewModel? model = await pizzaService.GetPizzaForEditAsync(id);

            if (model != null)
            {
                model.Doughs = await doughService.GetAllDoughsAsync();
                model.AvailableProducts = await productService.GetAllProductsAsync();
                model.ProductsId = await productService.GetProductsByPizzaIdAsync(id);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditPizzaViewModel editModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }

            try
            {
                await pizzaService.EditPizzaByIdAndEditModelAsync(id, editModel);
                TempData[SuccessMessage] = "Pizza is successfully edited!";

            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Unexpected error occured. Try later or contact administrator!";
                editModel.Doughs = await doughService.GetAllDoughsAsync();
                editModel.AvailableProducts = await productService.GetAllProductsAsync();
                editModel.ProductsId = await productService.GetProductsByPizzaIdAsync(id);
                return View(editModel);
            }

            return RedirectToAction("All", "Pizza", new { area = "" });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                DeletePizzaViewModel pizzaModel =
                    await pizzaService.GetPizzaForDeleteAsync(id);

                return View(pizzaModel);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Unexpected error occured. Try later or contact administrator!";
                return RedirectToAction("All", "Pizza", new { area = "" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, DeletePizzaViewModel pizzaModel)
        {
            try
            {
                await pizzaService.DeleteByIdAsync(id);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Unexpected error occured. Try later or contact administrator!";
                return View(pizzaModel);
            }

            TempData[SuccessMessage] = "Pizza is deleted successfully";
            return RedirectToAction("All", "Pizza", new { area = "" });
        }
    }
}
