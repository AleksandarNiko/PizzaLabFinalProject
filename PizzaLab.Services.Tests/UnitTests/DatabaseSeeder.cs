namespace PizzaLab.Services.Tests.UnitTests
{
    using PizzaLab.Data;
    using PizzaLab.Data.Models;

    public static class DatabaseSeeder
    {
        public static Menu MenuTest;
        public static Pizza PizzaTest;
        public static Topping ToppingTest;
        public static Dough DoughTest;
        public static Product ProductTest;

        public static void SeedDatabase(PizzaLabDbContext dbContext)
        {
            MenuTest = new Menu()
            {
                Name = "Test menu",
                Description = "Testing menu description",
            };
            dbContext.Menus.Add(MenuTest);


            PizzaTest = new Pizza()
            {
                Name = "Test pizza",
                InitialPrice = 12.99M,
                ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQSFF8PErjfcRq_lYAHhj2OrrqqTdY0FKohDA&usqp=CAU",
                Description = "Testing PIZZA description",
                DoughId = 1
            };
            dbContext.Pizzas.Add(PizzaTest);


            ToppingTest = new Topping()
            {
                Name = "Test toppnig",
                Price = 1.99M
            };
            dbContext.Toppings.Add(ToppingTest);


            ProductTest = new Product()
            {
                Name = "Test Product",
            };
            dbContext.Product.Add(ProductTest);


            DoughTest = new Dough()
            {
                Name = "Test Dough"
            };
            dbContext.Doughs.Add(DoughTest);

            dbContext.SaveChanges();
        }
    }
}
