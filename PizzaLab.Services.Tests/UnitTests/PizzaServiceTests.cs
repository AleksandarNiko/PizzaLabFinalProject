namespace PizzaLab.Services.Tests.UnitTests
{
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using PizzaLab.Data;
    using PizzaLab.Data.Models;
    using PizzaLab.Services.Data;
    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Web.ViewModels.Pizza;

    using static DatabaseSeeder;

    public class PizzaServiceTests
    {
        private DbContextOptions<PizzaLabDbContext> dbOptions;
        private PizzaLabDbContext dbContext;

        private IPizzaService pizzaService;
        private IDoughService dughService;
        private IProductService productService;
        private IToppingService toppingService;

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

            pizzaService = new PizzaService(dbContext, dughService, productService, toppingService);
        }

        [Test]
        public async Task AddPizzaAsyncShouldAddPizzaToDatabase()
        {
            var model = new AddPizzaViewModel()
            {
                Name = "Test Pizza",
                InitialPrice = 10.99M,
                ImageUrl = "test-image-url",
                Description = "Test Description",
                DoughId = DoughTest.Id,
                ProductsId = new List<int> { ProductTest.Id }
            };

            await pizzaService.AddPizzaAsync(model);

            var addedPizza = await dbContext.Pizzas.FirstOrDefaultAsync(p => p.Name == "Test Pizza");
            Assert.NotNull(addedPizza);
            Assert.AreEqual(model.InitialPrice, addedPizza.InitialPrice);
            Assert.AreEqual(model.ImageUrl, addedPizza.ImageUrl);
            Assert.AreEqual(model.Description, addedPizza.Description);

            var pizzaProduct = await dbContext.PizzasProducts.FirstOrDefaultAsync(pp => pp.PizzaId == addedPizza.Id);
            Assert.NotNull(pizzaProduct);
        }

        [Test]
        public async Task GetAllPizzasAsyncShouldReturnAllPizzas()
        {
            dbContext.Pizzas.RemoveRange(dbContext.Pizzas);
            dbContext.Pizzas.Add(PizzaTest);

            var pizzas = await pizzaService.GetAllPizzasAsync();

            Assert.NotNull(pizzas);
            Assert.IsTrue(pizzas.Any());

            foreach (var pizza in pizzas)
            {
                Assert.NotNull(pizza);
                Assert.NotNull(pizza.Products);
                Assert.NotNull(pizza.DoughName);
            }
        }

        [Test]
        public async Task GetPizzaByIdAsyncShouldReturnPizzaDetails()
        {
            var pizzaId = PizzaTest.Id;

            var pizzaDetails = await pizzaService.GetPizzaByIdAsync(pizzaId);

            Assert.NotNull(pizzaDetails);
            Assert.AreEqual(pizzaId, pizzaDetails.Id);
            Assert.AreEqual(PizzaTest.Name, pizzaDetails.Name);
            Assert.AreEqual(PizzaTest.InitialPrice, pizzaDetails.InitialPrice);
            Assert.AreEqual(PizzaTest.ImageUrl, pizzaDetails.ImageUrl);
            Assert.AreEqual(PizzaTest.Description, pizzaDetails.Description);
        }

        [Test]
        public async Task GetPizzaForEditAsyncShouldReturnEditPizzaViewModel()
        {
            var pizzaToEdit = PizzaTest;

            var editPizzaViewModel = await pizzaService.GetPizzaForEditAsync(pizzaToEdit.Id);

            Assert.NotNull(editPizzaViewModel);
            Assert.AreEqual(pizzaToEdit.Id, editPizzaViewModel.Id);
            Assert.AreEqual(pizzaToEdit.Name, editPizzaViewModel.Name);
            Assert.AreEqual(pizzaToEdit.InitialPrice, editPizzaViewModel.InitialPrice);
            Assert.AreEqual(pizzaToEdit.ImageUrl, editPizzaViewModel.ImageUrl);
            Assert.AreEqual(pizzaToEdit.Description, editPizzaViewModel.Description);
            Assert.AreEqual(pizzaToEdit.DoughId, editPizzaViewModel.DoughId);
        }

        [Test]
        public async Task GetPizzasByMenuIdAsyncShouldReturnPizzasForMenu()
        {
            var menu = MenuTest;
            var pizza = PizzaTest;
            var menuPizza = new MenuPizza { Menu = menu, Pizza = pizza };
            dbContext.MenusPizzas.Add(menuPizza);
            await dbContext.SaveChangesAsync();

            var pizzasForMenu = await pizzaService.GetPizzasByMenuIdAsync(menu.Id);

            Assert.NotNull(pizzasForMenu);
            Assert.IsTrue(pizzasForMenu.Any());

            foreach (var pizzaViewModel in pizzasForMenu)
            {
                Assert.NotNull(pizzaViewModel);
                Assert.NotNull(pizzaViewModel.Products);
                Assert.NotNull(pizzaViewModel.DoughName);
            }
        }

        [Test]
        public async Task EditPizzaByIdAndEditModelAsyncShouldUpdatePizzaCorrectly()
        {
            var initialPizzaName = "Initial Pizza";
            var editedPizzaName = "Edited Pizza";
            var initialDoughId = DoughTest.Id;
            var editedDoughId = initialDoughId + 1;

            var initialPizza = new Pizza
            {
                Name = initialPizzaName,
                InitialPrice = 10.99M,
                ImageUrl = "initial-image-url",
                Description = "Initial description",
                DoughId = initialDoughId
            };
            dbContext.Pizzas.Add(initialPizza);
            await dbContext.SaveChangesAsync();

            var editModel = new EditPizzaViewModel
            {
                Name = editedPizzaName,
                InitialPrice = 12.99M,
                ImageUrl = "edited-image-url",
                Description = "Edited description",
                DoughId = editedDoughId,
                ProductsId = new List<int> { ProductTest.Id }
            };

            await pizzaService.EditPizzaByIdAndEditModelAsync(initialPizza.Id, editModel);

            var editedPizza = await dbContext.Pizzas.FindAsync(initialPizza.Id);
            var editedPizzaProductsCount = await dbContext.PizzasProducts.CountAsync(pp => pp.PizzaId == initialPizza.Id);

            Assert.NotNull(editedPizza);
            Assert.AreEqual(editedPizzaName, editedPizza.Name);
            Assert.AreEqual(12.99M, editedPizza.InitialPrice);
            Assert.AreEqual("edited-image-url", editedPizza.ImageUrl);
            Assert.AreEqual("Edited description", editedPizza.Description);
            Assert.AreEqual(editedDoughId, editedPizza.DoughId);
            Assert.AreEqual(1, editedPizzaProductsCount);
        }

        [Test]
        public async Task GetPizzaForDeleteAsyncShouldReturnCorrectDeleteViewModel()
        {
            var pizzaToDeleteName = "Pizza to Delete";
            var pizzaToDeleteInitialPrice = 10.99M;
            var pizzaToDeleteImageUrl = "delete-image-url";
            var pizzaToDeleteDescription = "Delete description";

            var pizzaToDelete = new Pizza
            {
                Name = pizzaToDeleteName,
                InitialPrice = pizzaToDeleteInitialPrice,
                ImageUrl = pizzaToDeleteImageUrl,
                Description = pizzaToDeleteDescription,
                DoughId = DoughTest.Id
            };
            dbContext.Pizzas.Add(pizzaToDelete);
            await dbContext.SaveChangesAsync();

            var deleteViewModel = await pizzaService.GetPizzaForDeleteAsync(pizzaToDelete.Id);

            Assert.NotNull(deleteViewModel);
            Assert.AreEqual(pizzaToDeleteName, deleteViewModel.Name);
            Assert.AreEqual(pizzaToDeleteInitialPrice, deleteViewModel.InitialPrice);
            Assert.AreEqual(pizzaToDeleteImageUrl, deleteViewModel.ImageUrl);
            Assert.AreEqual(pizzaToDeleteDescription, deleteViewModel.Description);
        }

        [Test]
        public async Task DeleteByIdAsyncShouldDeletePizza()
        {
            var pizzaToDelete = new Pizza
            {
                Name = "Pizza to Delete",
                InitialPrice = 10.99M,
                ImageUrl = "delete-image-url",
                Description = "Delete description",
                DoughId = DoughTest.Id
            };
            dbContext.Pizzas.Add(pizzaToDelete);
            await dbContext.SaveChangesAsync();

            var initialPizzaCount = dbContext.Pizzas.Count();

            await pizzaService.DeleteByIdAsync(pizzaToDelete.Id);

            var remainingPizzas = dbContext.Pizzas.Count();

            Assert.AreEqual(initialPizzaCount - 1, remainingPizzas);
        }

    }
}
