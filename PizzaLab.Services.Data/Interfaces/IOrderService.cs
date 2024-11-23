namespace PizzaLab.Services.Data.Interfaces
{
    public interface IOrderService
    {
        Task AddOrderAsync(string userId);
        Task EmptyCartAsync(string userId);
        Task RemoveCartPizzasAsync(string userId);
    }
}
