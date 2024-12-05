namespace PizzaLab.Services.Tests.UnitTests
{
    using Microsoft.EntityFrameworkCore;

    using PizzaLab.Data;
    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Services.Data;

    using PizzaLab.Web.ViewModels.Products;
    using PizzaLab.Data.Models;

    using static DatabaseSeeder;
    using NUnit.Framework.Legacy;

    public class ProductServiceTests
    {
        private DbContextOptions<PizzaLabDbContext> dbOptions;
        private PizzaLabDbContext dbContext;

        private IProductService productService;

        [SetUp]
        public void OneTimeSetUp()
        {
            dbOptions = new DbContextOptionsBuilder<PizzaLabDbContext>()
                .UseInMemoryDatabase("PizzaLabInMemory" + Guid.NewGuid().ToString())
                .Options;
            dbContext = new PizzaLabDbContext(dbOptions);

            dbContext.Database.EnsureCreated();
            SeedDatabase(dbContext);

            productService = new ProductService(dbContext);
        }

        [Test]
        public async Task AddProductAsyncShouldAddProductToDatabase()
        {
            var initialProductCount = await dbContext.Product.CountAsync();

            var productModel = new AddProductViewModel
            {
                Name = "Test Product"
            };

            await productService.AddProductAsync(productModel);

            var addedProduct = await dbContext
                .Product
                .FirstOrDefaultAsync(p => p.Name == productModel.Name);
            ClassicAssert.NotNull(addedProduct);

            var finalProductCount = await dbContext.Product.CountAsync();
            ClassicAssert.AreEqual(initialProductCount + 1, finalProductCount);
        }

        [Test]
        public async Task DeleteByIdAsyncShouldRemoveProductFromDatabase()
        {
            var product = new Product
            {
                Name = "Product to Delete"
            };
            dbContext.Product.Add(product);
            await dbContext.SaveChangesAsync();

            var initialProductCount = await dbContext.Product.CountAsync();

            await productService.DeleteByIdAsync(product.Id);

            var deletedProduct = await dbContext
                .Product
                .FirstOrDefaultAsync(p => p.Id == product.Id);
            ClassicAssert.Null(deletedProduct);

            var finalProductCount = await dbContext.Product.CountAsync();
            ClassicAssert.AreEqual(initialProductCount - 1, finalProductCount);
        }

        [Test]
        public async Task GetAllProductsAsyncShouldReturnAllProducts()
        {
            var product1 = new Product
            {
                Name = "Product 1"
            };
            var product2 = new Product
            {
                Name = "Product 2"
            };
            dbContext.Product.RemoveRange(dbContext.Product);
            dbContext.Product.AddRange(product1, product2);
            await dbContext.SaveChangesAsync();

            var products = await productService.GetAllProductsAsync();

            ClassicAssert.NotNull(products);
            ClassicAssert.AreEqual(2, products.Count());

            var productViewModel1 = products.FirstOrDefault(p => p.Name == product1.Name);
            var productViewModel2 = products.FirstOrDefault(p => p.Name == product2.Name);

            ClassicAssert.NotNull(productViewModel1);
            ClassicAssert.NotNull(productViewModel2);
        }

        [Test]
        public async Task GetProductByIdAsyncShouldReturnProductById()
        {
            var product = new Product
            {
                Name = "Test Product"
            };
            dbContext.Product.Add(product);
            await dbContext.SaveChangesAsync();

            var productViewModel = await productService.GetProductByIdAsync(product.Id);

            ClassicAssert.NotNull(productViewModel);
            ClassicAssert.AreEqual(product.Id, productViewModel.Id);
            ClassicAssert.AreEqual(product.Name, productViewModel.Name);
        }

        [Test]
        public async Task GetProductByIdAsyncShouldReturnNullForNonExistentProduct()
        {
            var productViewModel = await productService.GetProductByIdAsync(-1);

            ClassicAssert.Null(productViewModel);
        }
    }
}

