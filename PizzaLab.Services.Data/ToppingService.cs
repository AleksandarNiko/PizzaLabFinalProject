namespace PizzaLab.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using PizzaLab.Data;
    using PizzaLab.Data.Models;
    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Web.ViewModels.Topping;

    public class ToppingService : IToppingService
    {
        private readonly PizzaLabDbContext dbContext;

        public ToppingService(PizzaLabDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }
        public async Task AddToppingAsync(AddToppingViewModel model)
        {
            Topping topping = new Topping()
            {
                Name = model.Name,
                Price = model.Price
            };

            await this.dbContext.Toppings.AddAsync(topping);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Topping toppingToDelete = await dbContext
                .Toppings
                .FirstAsync(x => x.Id == id);

            dbContext.Toppings.Remove(toppingToDelete);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ToppingForPizzaVIewModel>> GetAllToppingsAsync()
        {
            return await dbContext
                .Toppings
                .Select(p => new ToppingForPizzaVIewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                })
                .ToArrayAsync();
        }

        public async Task<ToppingForPizzaVIewModel> GetToppingByIdAsync(int toppingId)
        {
            Topping? topping = await this.dbContext
                .Toppings
                .FirstOrDefaultAsync(p => p.Id == toppingId);

            if (topping == null)
            {
                return null;
            }

            ToppingForPizzaVIewModel viewModel = new ToppingForPizzaVIewModel
            {
                Id = toppingId,
                Name = topping.Name,
                Price = topping.Price
            };

            return viewModel;
        }
    }
}
