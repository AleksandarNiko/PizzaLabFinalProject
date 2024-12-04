namespace PizzaLab.Services.Tests.UnitTests
{
    using Microsoft.EntityFrameworkCore;

    using PizzaLab.Data;
    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Services.Data;
    using PizzaLab.Web.ViewModels.Dough;
    using PizzaLab.Data.Models;

    using static DatabaseSeeder;
    using NUnit.Framework.Legacy;

    public class DoughServiceTests
    {
        private DbContextOptions<PizzaLabDbContext> dbOptions;
        private PizzaLabDbContext dbContext;

        private IDoughService doughService;

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
            ClassicAssert.NotNull(addedDough);
            ClassicAssert.AreEqual(doughName, addedDough.Name);
            ClassicAssert.AreEqual(firstCount + 1, dbContext.Doughs.Count());
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
            ClassicAssert.Null(deletedDough);
            ClassicAssert.AreEqual(firstCount, dbContext.Doughs.Count() + 1);
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

            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(doughs.Count, result.Count());

            foreach (var dough in doughs)
            {
                var viewModel = result.FirstOrDefault(d => d.Id == dough.Id);
                ClassicAssert.NotNull(viewModel);
                ClassicAssert.AreEqual(dough.Name, viewModel.Name);
            }
        }

        [Test]
        public async Task GetAllDoughsAsyncShouldReturnEmptyListWhenNoDoughs()
        {
            dbContext.Doughs.RemoveRange(dbContext.Doughs);
            await dbContext.SaveChangesAsync();

            var result = await doughService.GetAllDoughsAsync();

            ClassicAssert.NotNull(result);
            ClassicAssert.IsEmpty(result);
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

            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(doughId, result.Id);
            ClassicAssert.AreEqual(newDough.Name, result.Name);
        }
    }
}
