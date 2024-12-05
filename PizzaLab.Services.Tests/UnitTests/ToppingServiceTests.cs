namespace PizzaLab.Services.Tests.UnitTests
{
    using Microsoft.EntityFrameworkCore;
    using System;

    using PizzaLab.Data;
    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Services.Data;

    using PizzaLab.Web.ViewModels.Topping;
    using PizzaLab.Data.Models;

    using static DatabaseSeeder;
    using NUnit.Framework.Legacy;

    public class ToppingServiceTests
    {
        private DbContextOptions<PizzaLabDbContext> dbOptions;
        private PizzaLabDbContext dbContext;

        private IToppingService toppingService;

        [SetUp]
        public void OneTimeSetUp()
        {
            dbOptions = new DbContextOptionsBuilder<PizzaLabDbContext>()
                .UseInMemoryDatabase("PizzaLabInMemory" + Guid.NewGuid().ToString())
                .Options;
            dbContext = new PizzaLabDbContext(dbOptions);

            dbContext.Database.EnsureCreated();
            SeedDatabase(dbContext);

            toppingService = new ToppingService(dbContext);
        }
        [Test]
        public async Task AddToppingAsyncShouldAddToppingAndIncreaseCount()
        {
            var model = new AddToppingViewModel
            {
                Name = "New Topping",
                Price = 2.5M
            };

            var initialCount = await dbContext.Toppings.CountAsync();
            await toppingService.AddToppingAsync(model);
            var newCount = await dbContext.Toppings.CountAsync();

            ClassicAssert.AreEqual(initialCount + 1, newCount);
        }

        [Test]
        public async Task DeleteByIdAsyncShouldDeleteToppingAndDecreaseCount()
        {
            var topping = new Topping
            {
                Name = "Topping to Delete",
                Price = 3.0M
            };
            dbContext.Toppings.Add(topping);
            await dbContext.SaveChangesAsync();

            var initialCount = await dbContext.Toppings.CountAsync();
            await toppingService.DeleteByIdAsync(topping.Id);
            var newCount = await dbContext.Toppings.CountAsync();

            ClassicAssert.AreEqual(initialCount - 1, newCount);
        }


        [Test]
        public async Task GetToppingByIdAsyncShouldReturnCorrectTopping()
        {
            var topping = new Topping { Name = "Test Topping", Price = 2.0M };
            dbContext.Toppings.Add(topping);
            await dbContext.SaveChangesAsync();

            var retrievedTopping = await toppingService.GetToppingByIdAsync(topping.Id);

            ClassicAssert.NotNull(retrievedTopping);
            ClassicAssert.AreEqual(topping.Name, retrievedTopping.Name);
            ClassicAssert.AreEqual(topping.Price, retrievedTopping.Price);
        }

    }
}
