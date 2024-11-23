namespace PizzaLab.Services.Data.Interfaces
{
    using PizzaLab.Web.ViewModels.Products;

    public interface IProductService
    {
        Task AddProductAsync(AddProductViewModel model);
        Task DeleteByIdAsync(int id);
        Task<IEnumerable<ProductsForPizzaViewModel>> GetAllProductsAsync();
        Task<ProductsForPizzaViewModel?> GetProductByIdAsync(int productId);
        Task<IEnumerable<int>> GetProductsByPizzaIdAsync(int pizzaId);
    }
}
