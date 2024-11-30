using Microsoft.AspNetCore.Mvc;
using PizzaLab.Services.Data;
using PizzaLab.Web.ViewModels.Menu;
using PizzaLab.Services.Data.Interfaces;

using static PizzaLab.Common.NotificationMessagesConstants;

namespace PizzaLab.Web.Areas.Admin.Controllers
{

    public class MenuController : BaseAdminController
    {
        private readonly IMenuService menuService;
        private readonly IPizzaService pizzaService;

        public MenuController(IMenuService _menuService, IPizzaService _pizzaService)
        {
            this.menuService = _menuService;
            this.pizzaService = _pizzaService;

        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMenuViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await menuService.AddMenuAsync(model);

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                DeleteMenuViewModel menuModel =
                    await menuService.GetMenuForDeleteAsync(id);

                return View(menuModel);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Unexpected error occured. Try later or contact administrator!";
                return RedirectToAction("All", "Menu", new { area = "" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, DeleteMenuViewModel menuModel)
        {
            try
            {
                await menuService.DeleteByIdAsync(id);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Unexpected error occured. Try later or contact administrator!";
                return View(menuModel);
            }

            return RedirectToAction("All", "Menu", new { area = "" });
        }

        [HttpGet]
        public async Task<IActionResult> RemovePizzaFromMenu(int menuId, int pizzaId)
        {
            RemovePizzaFromMenuViewModel model =
                await menuService.GetRemovePizzaView(menuId, pizzaId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemovePizzaFromMenu(RemovePizzaFromMenuViewModel model)
        {

            try
            {
                await menuService.RemovePizzaFromMenuAsync(model.MenuId, model.PizzaId);
                TempData["SuccessMessage"] = "Pizza is successfully removed from the menu";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to remove pizza from the menu.";
                model = await menuService.GetRemovePizzaView(model.MenuId, model.PizzaId);
                return View(model);
            }

            return RedirectToAction("Edit", new { id = model.MenuId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            EditMenuViewModel model = await menuService.GetMenuForEditAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditMenuViewModel editModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }

            try
            {
                await menuService.EditMenuByIdAndEditModelAsync(id, editModel);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Unexpected error occured. Try later or contact administrator!";
                return View(editModel);
            }

            return RedirectToAction("All", "Menu", new { area = "" });
        }


        [HttpGet]
        public async Task<IActionResult> AddPizzas(int menuId)
        {
            var model = await pizzaService.GetAllPizzasWithDifferentMenuIdAsync(menuId);
            ViewBag.MenuId = menuId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPizzaToMenu(int menuId, int pizzaId)
        {
            var success = await menuService.AddPizzaToMenuAsync(menuId, pizzaId);

            if (!success)
            {
                TempData["ErrorMessage"] = "Pizza or menu Id are not correct";
                return RedirectToAction("AddPizzas", new { menuId, area = "Admin" });
            }

            TempData["SuccessMessage"] = "Pizza successfully added in the menu";
            return RedirectToAction("AddPizzas", new { menuId, area = "Admin" });
        }
    }
}
