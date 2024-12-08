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
    using NUnit.Framework;

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

            Assert.IsTrue(result);
        }

        [Test]
        public async Task MenuExistsByMenuIdShouldReturnFalseWhenAbsent()
        {
            int existingMenuId = MenuTest.Id;

            bool result = await menuService.ExistsByIdAsync(int.MaxValue);

            Assert.IsFalse(result);
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
            Assert.NotNull(addedMenu);
            Assert.AreEqual(model.Name, addedMenu.Name);
            Assert.AreEqual(model.Description, addedMenu.Description);
        }

        [Test]
        public async Task DeleteByIdAsyncShouldDeleteMenu()
        {
            int existingMenuId = MenuTest.Id;

            await menuService.DeleteByIdAsync(existingMenuId);

            Assert.IsFalse(await dbContext.Menus.AnyAsync(m => m.Id == existingMenuId));
        }

        [Test]
        public async Task DeleteByIdAsyncShouldNotDeleteNonExistingMenu()
        {
            int nonExistingMenuId = int.MaxValue;
            int initialMenuCount = dbContext.Menus.Count();

            await menuService.DeleteByIdAsync(nonExistingMenuId);

            var remainingMenus = await dbContext.Menus.CountAsync();
            Assert.AreEqual(initialMenuCount, remainingMenus);
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
            Assert.NotNull(editedMenu);
            Assert.AreEqual(editModel.Name, editedMenu.Name);
            Assert.AreEqual(editModel.Description, editedMenu.Description);
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

            Assert.AreEqual(expectedMenus.Count, result.Count());

            foreach (var expectedMenu in expectedMenus)
            {
                var actualMenu = result.FirstOrDefault(m => m.Id == expectedMenu.Id);
                Assert.NotNull(actualMenu);
                Assert.AreEqual(expectedMenu.Name, actualMenu.Name);
                Assert.AreEqual(expectedMenu.Description, actualMenu.Description);
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

            Assert.AreEqual(pizzasForMenu.Count(), result.Count());

            foreach (var expectedPizza in pizzasForMenu)
            {
                var actualPizza = result.FirstOrDefault(p => p.Id == expectedPizza.Id);
                Assert.NotNull(actualPizza);
                Assert.AreEqual(expectedPizza.Name, actualPizza.Name);
                Assert.AreEqual(expectedPizza.InitialPrice, actualPizza.InitialPrice);
                Assert.AreEqual(expectedPizza.ImageUrl, actualPizza.ImageUrl);
                Assert.AreEqual(expectedPizza.Description, actualPizza.Description);
            }
        }

        [Test]
        public async Task GetMenuForDeleteAsyncShouldReturnMenuForDeleteViewModel()
        {
            var menuId = MenuTest.Id;

            var menuService = new MenuService(dbContext);

            var deleteMenuViewModel = await menuService.GetMenuForDeleteAsync(menuId);

            Assert.NotNull(deleteMenuViewModel);
            Assert.AreEqual("Test menu", deleteMenuViewModel.Name);
            Assert.AreEqual("Testing menu description", deleteMenuViewModel.Description);

        }

        [Test]
        public async Task GetMenuForEditAsyncShouldReturnEditMenuViewModel()
        {
            var menuId = MenuTest.Id;

            var editMenuViewModel = await menuService.GetMenuForEditAsync(menuId);

            Assert.NotNull(editMenuViewModel);
            Assert.AreEqual("Test menu", editMenuViewModel.Name);
            Assert.AreEqual("Testing menu description", editMenuViewModel.Description);

            var menuPizzas = await menuService.GetAllPizzasByMenuIdAsync(menuId);
            Assert.AreEqual(menuPizzas.Count(), editMenuViewModel.MenuPizzas.Count());
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

                Assert.NotNull(updatedMenu);
                Assert.IsFalse(updatedMenu.MenusPizzas.Any(mp => mp.PizzaId == pizzaId));
            }
        }

        [Test]
        public async Task AddPizzaToMenuAsyncShouldAddPizzaToMenu()
        {
            var menuId = MenuTest.Id;
            var pizzaId = PizzaTest.Id;

            var result = await menuService.AddPizzaToMenuAsync(menuId, pizzaId);

            Assert.IsTrue(result);

            using (var dbContext = new PizzaLabDbContext(dbOptions))
            {
                var updatedMenu = await dbContext.Menus
                    .Include(m => m.MenusPizzas)
                    .FirstOrDefaultAsync(m => m.Id == menuId);

                Assert.NotNull(updatedMenu);
                Assert.IsTrue(updatedMenu.MenusPizzas.Any(mp => mp.PizzaId == pizzaId));
            }
        }

        [Test]
        public async Task GetRemovePizzaViewShouldReturnRemovePizzaFromMenuViewModel()
        {
            var menuId = MenuTest.Id;
            var pizzaId = PizzaTest.Id;

            var result = await menuService.GetRemovePizzaView(menuId, pizzaId);

            Assert.NotNull(result);
            Assert.AreEqual(menuId, result.MenuId);
            Assert.AreEqual(pizzaId, result.PizzaId);
            Assert.AreEqual(MenuTest.Name, result.MenuName);
            Assert.AreEqual(PizzaTest.Name, result.PizzaName);
        }

        [Test]
        public async Task GetStatisticsAsyncShouldReturnStatisticsServiceModel()
        {
            var result = await menuService.GetStatisticsAsync();

            Assert.NotNull(result);
            Assert.AreEqual(await dbContext.Pizzas.CountAsync(), result.TotalPizzas);
            Assert.AreEqual(await dbContext.Menus.CountAsync(), result.TotalMenus);
        }

    }
}