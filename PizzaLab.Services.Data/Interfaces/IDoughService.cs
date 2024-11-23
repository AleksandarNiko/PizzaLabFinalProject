namespace  PizzaLab.Services.Data.Interfaces
{
    using PizzaLab.Web.ViewModels.Dough;

    public interface IDoughService
    {
        Task AddDoughAsync(AddDoughViewModel model);
        Task DeleteByIdAsync(int id);
        Task<IEnumerable<DoughViewModel>> GetAllDoughsAsync();
        Task<DoughViewModel?> GetDoughByIdAsync(int doughId);
    }
}
