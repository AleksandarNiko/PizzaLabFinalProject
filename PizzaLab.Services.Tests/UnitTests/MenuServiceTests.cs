namespace PizzaLab.Services.Tests.UnitTests
{
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework.Internal;
    using NUnit.Framework;

    using PizzaLab.Data;
    using PizzaLab.Services.Data;
    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Web.ViewModels.Menu;
    using PizzaLab.Data.Models;

    using static DatabaseSeeder;
    using NUnit.Framework.Legacy;

    public class MenuServiceTests
    {
        private DbContextOptions<PizzaLabDbContext> dbOptions;
        private PizzaLabDbContext dbContext;

        private IMenuService menuService;

        [TearDown]
        public async Task Teardown()
        {
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.DisposeAsync();

        }

        [SetUp]
        public void OneTimeSetUp()
        {
            dbOptions = new DbContextOptionsBuilder<PizzaLabDbContext>()
                .UseInMemoryDatabase("PizzaLabInMemory" + Guid.NewGuid().ToString())
                .Options;
            dbContext = new PizzaLabDbContext(dbOptions);

            dbContext.Database.EnsureCreated();
            SeedDatabase(dbContext);

            menuService = new MenuService(dbContext);
        }

        [Test]
        public async Task MenuExistsByMenuIdShouldReturnTrueWhenExists()
        {
            int existingMenuId = MenuTest.Id;

            bool result = await menuService.ExistsByIdAsync(existingMenuId);

            ClassicAssert.IsTrue(result);
        }

        [Test]
        public async Task MenuExistsByMenuIdShouldReturnFalseWhenAbsent()
        {
            int existingMenuId = MenuTest.Id;

            bool result = await menuService.ExistsByIdAsync(int.MaxValue);

            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task AddMenuShouldAddMenuToDatabase()
        {
            var model = new AddMenuViewModel
            {
                Name = "Test Menu to add",
                Description = "Test Description to add"
            };

            await menuService.AddMenuAsync(model);

            var addedMenu = await dbContext.Menus.FirstOrDefaultAsync(m => m.Name == model.Name);
            ClassicAssert.NotNull(addedMenu);
            ClassicAssert.AreEqual(model.Name, addedMenu.Name);
            ClassicAssert.AreEqual(model.Description, addedMenu.Description);
        }

        [Test]
        public async Task DeleteByIdAsyncShouldDeleteMenu()
        {
            int existingMenuId = MenuTest.Id;

            await menuService.DeleteByIdAsync(existingMenuId);

            ClassicAssert.IsFalse(await dbContext.Menus.AnyAsync(m => m.Id == existingMenuId));
        }

        [Test]
        public async Task DeleteByIdAsyncShouldNotDeleteNonExistingMenu()
        {
            int nonExistingMenuId = int.MaxValue;
            int initialMenuCount = dbContext.Menus.Count();

            await menuService.DeleteByIdAsync(nonExistingMenuId);

            var remainingMenus = await dbContext.Menus.CountAsync();
            ClassicAssert.AreEqual(initialMenuCount, remainingMenus);
        }

        [Test]
        public async Task EditMenuByIdAndEditModelAsyncShouldEditExistingMenu()
        {
            int existingMenuId = MenuTest.Id;
            var editModel = new EditMenuViewModel
            {
                Name = "Edited Menu Name",
                Description = "Edited Menu Description",
            };

            await menuService.EditMenuByIdAndEditModelAsync(existingMenuId, editModel);


            Menu? editedMenu = await dbContext.Menus.FirstOrDefaultAsync(m => m.Id == existingMenuId);
            ClassicAssert.NotNull(editedMenu);
            ClassicAssert.AreEqual(editModel.Name, editedMenu.Name);
            ClassicAssert.AreEqual(editModel.Description, editedMenu.Description);
        }

        [Test]
        public async Task GetAllMenusAsyncShouldReturnAllMenus()
        {
            dbContext.Menus.RemoveRange(dbContext.Menus);
            await dbContext.SaveChangesAsync();

            var expectedMenus = new List<Menu>
            {
                new Menu { Id = 1, Name = "Menu 1", Description = "Description 1" },
                new Menu { Id = 2, Name = "Menu 2", Description = "Description 2" },
            };

            dbContext.ChangeTracker.Clear();

            dbContext.Menus.AddRange(expectedMenus);
            await dbContext.SaveChangesAsync();

            var result = await menuService.GetAllMenusAsync();

            ClassicAssert.AreEqual(expectedMenus.Count, result.Count());

            foreach (var expectedMenu in expectedMenus)
            {
                var actualMenu = result.FirstOrDefault(m => m.Id == expectedMenu.Id);
                ClassicAssert.NotNull(actualMenu);
                ClassicAssert.AreEqual(expectedMenu.Name, actualMenu.Name);
                ClassicAssert.AreEqual(expectedMenu.Description, actualMenu.Description);
            }
        }

        [Test]
        public async Task GetAllPizzasByMenuIdAsyncShouldReturnPizzasForMenu()
        {
            Menu menu = MenuTest;
            Pizza pizza = PizzaTest;

            var pizzasForMenu = new List<Pizza>();
            pizzasForMenu.Add(pizza);

            dbContext.MenusPizzas.Add(new MenuPizza() { Menu = menu, Pizza = pizza });
            await dbContext.SaveChangesAsync();

            var result = await menuService.GetAllPizzasByMenuIdAsync(menu.Id);

            ClassicAssert.AreEqual(pizzasForMenu.Count(), result.Count());

            foreach (var expectedPizza in pizzasForMenu)
            {
                var actualPizza = result.FirstOrDefault(p => p.Id == expectedPizza.Id);
                ClassicAssert.NotNull(actualPizza);
                ClassicAssert.AreEqual(expectedPizza.Name, actualPizza.Name);
                ClassicAssert.AreEqual(expectedPizza.InitialPrice, actualPizza.InitialPrice);
                ClassicAssert.AreEqual(expectedPizza.ImageUrl, actualPizza.ImageUrl);
                ClassicAssert.AreEqual(expectedPizza.Description, actualPizza.Description);
            }
        }

        [Test]
        public async Task GetMenuForDeleteAsyncShouldReturnMenuForDeleteViewModel()
        {
            var menuId = MenuTest.Id;

            var menuService = new MenuService(dbContext);

            var deleteMenuViewModel = await menuService.GetMenuForDeleteAsync(menuId);

            ClassicAssert.NotNull(deleteMenuViewModel);
            ClassicAssert.AreEqual("Test menu", deleteMenuViewModel.Name);
            ClassicAssert.AreEqual("Testing menu description", deleteMenuViewModel.Description);

        }

        [Test]
        public async Task GetMenuForEditAsyncShouldReturnEditMenuViewModel()
        {
            var menuId = MenuTest.Id;

            var editMenuViewModel = await menuService.GetMenuForEditAsync(menuId);

            ClassicAssert.NotNull(editMenuViewModel);
            ClassicAssert.AreEqual("Test menu", editMenuViewModel.Name);
            ClassicAssert.AreEqual("Testing menu description", editMenuViewModel.Description);

            var menuPizzas = await menuService.GetAllPizzasByMenuIdAsync(menuId);
            ClassicAssert.AreEqual(menuPizzas.Count(), editMenuViewModel.MenuPizzas.Count());
        }

        [Test]
        public async Task RemovePizzaFromMenuAsyncShouldRemovePizzaFromMenu()
        {
            var menuId = MenuTest.Id;
            var pizzaId = PizzaTest.Id;

            dbContext.MenusPizzas.Add(new MenuPizza { Menu = MenuTest, Pizza = PizzaTest });
            await dbContext.SaveChangesAsync();

            await menuService.RemovePizzaFromMenuAsync(menuId, pizzaId);
            await dbContext.SaveChangesAsync();

            using (var dbContext = new PizzaLabDbContext(dbOptions))
            {
                var updatedMenu = await dbContext.Menus
                    .Include(m => m.MenusPizzas)
                    .FirstOrDefaultAsync(m => m.Id == menuId);

                ClassicAssert.NotNull(updatedMenu);
                ClassicAssert.IsFalse(updatedMenu.MenusPizzas.Any(mp => mp.PizzaId == pizzaId));
            }
        }

        [Test]
        public async Task AddPizzaToMenuAsyncShouldAddPizzaToMenu()
        {
            var menuId = MenuTest.Id;
            var pizzaId = PizzaTest.Id;

            var result = await menuService.AddPizzaToMenuAsync(menuId, pizzaId);

            ClassicAssert.IsTrue(result);

            using (var dbContext = new PizzaLabDbContext(dbOptions))
            {
                var updatedMenu = await dbContext.Menus
                    .Include(m => m.MenusPizzas)
                    .FirstOrDefaultAsync(m => m.Id == menuId);

                ClassicAssert.NotNull(updatedMenu);
                ClassicAssert.IsTrue(updatedMenu.MenusPizzas.Any(mp => mp.PizzaId == pizzaId));
            }
        }

        [Test]
        public async Task GetRemovePizzaViewShouldReturnRemovePizzaFromMenuViewModel()
        {
            var menuId = MenuTest.Id;
            var pizzaId = PizzaTest.Id;

            var result = await menuService.GetRemovePizzaView(menuId, pizzaId);

            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(menuId, result.MenuId);
            ClassicAssert.AreEqual(pizzaId, result.PizzaId);
            ClassicAssert.AreEqual(MenuTest.Name, result.MenuName);
            ClassicAssert.AreEqual(PizzaTest.Name, result.PizzaName);
        }

        [Test]
        public async Task GetStatisticsAsyncShouldReturnStatisticsServiceModel()
        {
            var result = await menuService.GetStatisticsAsync();

            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(await dbContext.Pizzas.CountAsync(), result.TotalPizzas);
            ClassicAssert.AreEqual(await dbContext.Menus.CountAsync(), result.TotalMenus);
        }

    }
}