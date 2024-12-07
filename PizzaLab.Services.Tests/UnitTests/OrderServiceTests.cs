namespace PizzaLab.Services.Tests.UnitTests
{
    using Microsoft.EntityFrameworkCore;
    using PizzaLab.Data;
    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Services.Data;

    using static DatabaseSeeder;
    using NUnit.Framework.Legacy;

    public class OrderServiceTests
    {
        private DbContextOptions<PizzaLabDbContext> dbOptions;
        private PizzaLabDbContext dbContext;

        private IOrderService orderService;
        private ICartService cartService;

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

            cartService = new CartService(dbContext);
            orderService = new OrderService(dbContext, cartService);
        }

        [Test]
        public async Task AddOrderAsyncShouldAddOrderWithCorrectPrice()
        {
            var userId = "59df8a72-7c6e-4c32-9b1e-eb1d07d17f79";
            await cartService.AddPizzaToCartAsync(1, 14.99M, userId);

            await orderService.AddOrderAsync(userId);

            var order = await dbContext
                .Orders
                .FirstOrDefaultAsync();
            ClassicAssert.NotNull(order);
            ClassicAssert.AreEqual(14.99M, order.Price);
        }

        [Test]
        public async Task EmptyCartAsyncShouldRemoveCartsForUser()
        {
            var userId = "59df8a72-7c6e-4c32-9b1e-eb1d07d17f79";
            await cartService.AddPizzaToCartAsync(1, 14.99M, userId);

            await orderService.EmptyCartAsync(userId);

            var cartsCount = await dbContext
                .Carts
                .CountAsync(c => c.UserId == Guid.Parse(userId));
            ClassicAssert.AreEqual(0, cartsCount);
        }

        [Test]
        public async Task RemoveCartPizzasAsyncShouldRemoveCartPizzasForUser()
        {
            var userId = "59df8a72-7c6e-4c32-9b1e-eb1d07d17f79";
            await cartService.AddPizzaToCartAsync(1, 14.99M, userId);

            await orderService.RemoveCartPizzasAsync(userId);

            var cartPizzasCount = await dbContext
                .CartsPizzas
                .CountAsync(cp => cp.UserId == Guid.Parse(userId));
            ClassicAssert.AreEqual(0, cartPizzasCount);
        }
    }
}

