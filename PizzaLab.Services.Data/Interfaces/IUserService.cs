namespace PizzaLab.Services.Data.Interfaces
{
    using PizzaLab.Web.ViewModels.User;

    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> AllAsync();
    }
}
