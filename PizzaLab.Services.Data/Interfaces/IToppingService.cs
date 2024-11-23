namespace PizzaLab.Services.Data.Interfaces
{
    using PizzaLab.Web.ViewModels.Topping;

    public interface IToppingService
    {
        Task AddToppingAsync(AddToppingViewModel model);
        Task DeleteByIdAsync(int id);
        Task<IEnumerable<ToppingForPizzaVIewModel>> GetAllToppingsAsync();
        Task<ToppingForPizzaVIewModel> GetToppingByIdAsync(int toppingId);
    }
}
