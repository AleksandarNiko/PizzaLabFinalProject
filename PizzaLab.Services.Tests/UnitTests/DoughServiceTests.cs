namespace PizzaLab.Services.Tests.UnitTests
{
    using Microsoft.EntityFrameworkCore;

    using PizzaLab.Data;
    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Services.Data;
    using PizzaLab.Web.ViewModels.Dough;
    using PizzaLab.Data.Models;

    using static DatabaseSeeder;
    using NUnit.Framework;

    public class DoughServiceTests
    {
        private DbContextOptions<PizzaLabDbContext> dbOptions;
        private PizzaLabDbContext dbContext;

        private IDoughService doughService;

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
                .UseInMemoryDatabase("PiizzaLabInMemory" + Guid.NewGuid().ToString())
                .Options;
            dbContext = new PizzaLabDbContext(dbOptions);

            dbContext.Database.EnsureCreated();
            SeedDatabase(dbContext);

            doughService = new DoughService(dbContext);
        }

        [Test]
        public async Task AddDoughAsync_ShouldAddDough()
        {
            var doughName = "Test Dough";
            int firstCount = dbContext.Doughs.Count();

            await doughService.AddDoughAsync(new AddDoughViewModel { Name = doughName });

            var addedDough = await dbContext.Doughs.FirstOrDefaultAsync(d => d.Name == doughName);
           Assert.NotNull(addedDough);
            Assert.AreEqual(doughName, addedDough.Name);
            Assert.AreEqual(firstCount + 1, dbContext.Doughs.Count());
        }

        [Test]
        public async Task DeleteDoughByIdAsyncShouldDeleteDough()
        {
            var doughName = "Dough to Delete";
            var dough = new Dough { Name = doughName };
            dbContext.Doughs.Add(dough);
            await dbContext.SaveChangesAsync();
            int firstCount = dbContext.Doughs.Count();

            await doughService.DeleteByIdAsync(dough.Id);

            var deletedDough = await dbContext.Doughs.FirstOrDefaultAsync(d => d.Id == dough.Id);
            Assert.Null(deletedDough);
            Assert.AreEqual(firstCount, dbContext.Doughs.Count() + 1);
        }

        [Test]
        public async Task GetAllDoughsAsyncShouldReturnAllDoughs()
        {
            var doughs = new List<Dough>
            {
                new Dough { Name = "Dough 1" },
                new Dough { Name = "Dough 2" },
                new Dough { Name = "Dough 3" }
            };
            dbContext.Doughs.RemoveRange(dbContext.Doughs);
            dbContext.Doughs.AddRange(doughs);
            await dbContext.SaveChangesAsync();

            var result = await doughService.GetAllDoughsAsync();

            Assert.NotNull(result);
            Assert.AreEqual(doughs.Count, result.Count());

            foreach (var dough in doughs)
            {
                var viewModel = result.FirstOrDefault(d => d.Id == dough.Id);
                Assert.NotNull(viewModel);
                Assert.AreEqual(dough.Name, viewModel.Name);
            }
        }

        [Test]
        public async Task GetAllDoughsAsyncShouldReturnEmptyListWhenNoDoughs()
        {
            dbContext.Doughs.RemoveRange(dbContext.Doughs);
            await dbContext.SaveChangesAsync();

            var result = await doughService.GetAllDoughsAsync();

            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetDoughByIdAsyncShouldReturnCorrectDough()
        {
            var newDough = new Dough
            {
                Name = "Test Dough"
            };
            dbContext.Doughs.Add(newDough);
            await dbContext.SaveChangesAsync();

            var doughId = newDough.Id;
            var result = await doughService.GetDoughByIdAsync(doughId);

            Assert.NotNull(result);
            Assert.AreEqual(doughId, result.Id);
            Assert.AreEqual(newDough.Name, result.Name);
        }
    }
}
