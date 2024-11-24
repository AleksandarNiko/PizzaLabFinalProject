namespace PizzaLab.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    using PizzaLab.Data;
    using PizzaLab.Data.Models;
    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Web.ViewModels.Dough;

    public class DoughService : IDoughService
    {
        private readonly PizzaLabDbContext dbContext;

        public DoughService(PizzaLabDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        public async Task AddDoughAsync(AddDoughViewModel model)
        {
            Dough dough = new Dough()
            {
                Name = model.Name,
            };

            await this.dbContext.Doughs.AddAsync(dough);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Dough doughToDelete = await dbContext
                .Doughs
                .FirstAsync(x => x.Id == id);

            dbContext.Doughs.Remove(doughToDelete);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DoughViewModel>> GetAllDoughsAsync()
        {
            return await dbContext
                .Doughs
                .Select(d => new DoughViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToArrayAsync();
        }

        public async Task<DoughViewModel?> GetDoughByIdAsync(int doughId)
        {
            var dough = await dbContext
                .Doughs
                .FirstOrDefaultAsync(d => d.Id == doughId);

            if (dough != null)
            {
                return new DoughViewModel
                {
                    Id = dough.Id,
                    Name = dough.Name
                };
            }

            return null;
        }
    }
}
