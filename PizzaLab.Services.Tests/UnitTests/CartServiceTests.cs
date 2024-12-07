namespace PizzaLab.Services.Tests.UnitTests
{
    using Microsoft.EntityFrameworkCore;
    using PizzaLab.Data;
    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Services.Data;

    using PizzaLab.Data.Models;

    using static DatabaseSeeder;
    using NUnit.Framework.Legacy;

    public class CartServiceTests
    {
        private DbContextOptions<PizzaLabDbContext> dbOptions;
        private PizzaLabDbContext dbContext;

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
        }

        [Test]
        public async Task AddPizzaToCartAsyncShouldAddPizzaToCart()
        {
            var userId = "12345678-1234-1234-1234-123456789012";
            var pizzaForCart = new Pizza
            {
                Name = "Pizza for Cart",
                InitialPrice = 10.99M,
                ImageUrl = "cart-image-url",
                Description = "Cart description",
                DoughId = DoughTest.Id
            };
            dbContext.Pizzas.Add(pizzaForCart);
            await dbContext.SaveChangesAsync();
            var updatedTotalPrice = 15.99M;

            await cartService.AddPizzaToCartAsync(pizzaForCart.Id, updatedTotalPrice, userId);

            var cart = await dbContext.Carts.FirstOrDefaultAsync(c => c.UserId == Guid.Parse(userId));
            ClassicAssert.NotNull(cart);
            ClassicAssert.AreEqual(updatedTotalPrice, cart.FinalPrice);

            var cartPizza = await dbContext.CartsPizzas.FirstOrDefaultAsync(cp => cp.CartId == cart.Id && cp.PizzaId == pizzaForCart.Id);
            ClassicAssert.NotNull(cartPizza);
            ClassicAssert.AreEqual(cart.Id, cartPizza.CartId);
            ClassicAssert.AreEqual(pizzaForCart.Id, cartPizza.PizzaId);
            ClassicAssert.AreEqual(Guid.Parse(userId), cartPizza.UserId);
            ClassicAssert.AreEqual(updatedTotalPrice, cartPizza.UpdatedPrice);
        }

        [Test]
        public async Task GetAllCartItemsAsyncShouldReturnAllCartItems()
        {
            var userId = "12345678-1234-1234-1234-123456789012";
            var cart = new Cart
            {
                UserId = Guid.Parse(userId),
                FinalPrice = 0.0M
            };
            dbContext.Carts.Add(cart);
            await dbContext.SaveChangesAsync();

            var pizza = new Pizza
            {
                Name = "Test Pizza",
                InitialPrice = 10.0M,
                ImageUrl = "test-image-url",
                Description = "Test description",
                DoughId = DoughTest.Id
            };
            dbContext.Pizzas.Add(pizza);
            await dbContext.SaveChangesAsync();

            var cartPizza = new CartPizza
            {
                Cart = cart,
                Pizza = pizza,
                UserId = Guid.Parse(userId),
                UpdatedPrice = 10.0M
            };
            dbContext.CartsPizzas.Add(cartPizza);
            await dbContext.SaveChangesAsync();

            var cartItems = await cartService.GetAllCartItemsAsync(userId);

            ClassicAssert.NotNull(cartItems);
            ClassicAssert.IsTrue(cartItems.Any());

            var cartItem = cartItems.First();
            ClassicAssert.AreEqual(Guid.Parse(userId), cartItem.UserId);
            ClassicAssert.AreEqual(pizza.Id, cartItem.PizzaId);
            ClassicAssert.AreEqual(cart.Id, cartItem.CartId);
            ClassicAssert.AreEqual(pizza.Name, cartItem.PizzaName);
            ClassicAssert.AreEqual(cartPizza.UpdatedPrice, cartItem.Price);
        }

        [Test]
        public async Task GetFinalPrizeAsyncShouldReturnCorrectFinalPrice()
        {
            var userId = "12345678-1234-1234-1234-123456789012";
            var cart = new Cart
            {
                UserId = Guid.Parse(userId),
                FinalPrice = 0.0M
            };
            dbContext.Carts.Add(cart);
            await dbContext.SaveChangesAsync();

            var pizza1 = new Pizza
            {
                Name = "Pizza 1",
                InitialPrice = 10.0M,
                ImageUrl = "image-url-1",
                Description = "Description 1",
                DoughId = DoughTest.Id
            };
            dbContext.Pizzas.Add(pizza1);
            await dbContext.SaveChangesAsync();

            var cartPizza1 = new CartPizza
            {
                Cart = cart,
                Pizza = pizza1,
                UserId = Guid.Parse(userId),
                UpdatedPrice = 10.0M
            };
            dbContext.CartsPizzas.Add(cartPizza1);
            await dbContext.SaveChangesAsync();

            var pizza2 = new Pizza
            {
                Name = "Pizza 2",
                InitialPrice = 15.0M,
                ImageUrl = "image-url-2",
                Description = "Description 2",
                DoughId = DoughTest.Id
            };
            dbContext.Pizzas.Add(pizza2);
            await dbContext.SaveChangesAsync();

            var cartPizza2 = new CartPizza
            {
                Cart = cart,
                Pizza = pizza2,
                UserId = Guid.Parse(userId),
                UpdatedPrice = 15.0M
            };
            dbContext.CartsPizzas.Add(cartPizza2);
            await dbContext.SaveChangesAsync();

            var finalPrice = await cartService.GetFinalPrizeAsync(userId);

            ClassicAssert.AreEqual(pizza1.InitialPrice + pizza2.InitialPrice, finalPrice);
        }

        [Test]
        public async Task RemovePizzaFromCartAsyncShouldRemovePizzaFromCart()
        {
            var userId = "12345678-1234-1234-1234-123456789012";
            var cart = new Cart
            {
                UserId = Guid.Parse(userId),
                FinalPrice = 0.0M
            };
            dbContext.Carts.Add(cart);
            await dbContext.SaveChangesAsync();

            var pizza = new Pizza
            {
                Name = "Pizza",
                InitialPrice = 10.0M,
                ImageUrl = "image-url",
                Description = "Description",
                DoughId = DoughTest.Id
            };
            dbContext.Pizzas.Add(pizza);
            await dbContext.SaveChangesAsync();

            var cartPizza = new CartPizza
            {
                Cart = cart,
                Pizza = pizza,
                UserId = Guid.Parse(userId),
                UpdatedPrice = 10.0M
            };
            dbContext.CartsPizzas.Add(cartPizza);
            await dbContext.SaveChangesAsync();

            await cartService.RemovePizzaFromCartAsync(cart.Id, pizza.Id, userId);

            var removedCartPizza = await dbContext.CartsPizzas.FirstOrDefaultAsync(cp => cp.CartId == cart.Id && cp.PizzaId == pizza.Id);
            ClassicAssert.Null(removedCartPizza);
        }
    }
}
