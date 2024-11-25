namespace PizzaLab.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    using PizzaLab.Data;
    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Web.ViewModels.User;

    public class UserService : IUserService
    {
        private readonly PizzaLabDbContext dbContext;

        public UserService(PizzaLabDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<UserViewModel>> AllAsync()
        {
           
        List<UserViewModel> allUsers = await this.dbContext
                   .Users
                   .Select(u => new UserViewModel()
                   {
                       Id = u.Id.ToString(),
                       Email = u.Email,
                   })
                   .ToListAsync();

                return allUsers;
        }
    }
}
