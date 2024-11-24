namespace PizzaLab.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using PizzaLab.Data;
    using PizzaLab.Data.Models;
    using PizzaLab.Services.Data.Interfaces;
    using PizzaLab.Web.ViewModels.Pizza;
    using PizzaLab.Web.ViewModels.Products;

    public class PizzaService : IPizzaService
    {
        private readonly PizzaLabDbContext dbContext;
        private readonly IDoughService doughService;
        private readonly IProductService productService;
        private readonly IToppingService toppingService;

        public PizzaService(PizzaLabDbContext _dbContext, IDoughService _doughService, IProductService _productService, IToppingService _toppingService)
        {
            this.dbContext = _dbContext;
            this.doughService = _doughService;
            this.productService = _productService;
            this.toppingService = _toppingService;
        }

        public async Task AddPizzaAsync(AddPizzaViewModel model)
        {
            Dough? dough = await dbContext
                .Doughs
                .FirstOrDefaultAsync(d => d.Id == model.DoughId);

            if (dough != null)
            {
                Pizza pizza = new Pizza()
                {
                    Name = model.Name,
                    InitialPrice = model.InitialPrice,
                    ImageUrl = model.ImageUrl,
                    Description = model.Description,
                    DoughId = dough.Id
                };

                await dbContext.Pizzas.AddAsync(pizza);
                await dbContext.SaveChangesAsync();

                foreach (int productId in model.ProductsId)
                {
                    Product? product = await dbContext
                        .Product
                        .FirstOrDefaultAsync(p => p.Id == productId);

                    if (product != null)
                    {
                        PizzaProduct pizzaProduct = new PizzaProduct()
                        {
                            PizzaId = pizza.Id,
                            ProductId = productId
                        };
                        await dbContext.PizzasProducts.AddAsync(pizzaProduct);
                    }
                }

                await dbContext.SaveChangesAsync();             
            }
        }

        public async Task<IEnumerable<PizzasForMenuViewModel>> GetAllPizzasWithDifferentMenuIdAsync(int id)
        {
            return await dbContext
                    .Pizzas
                    .Where(p => !p.MenusPizzas.Any(mp => mp.MenuId == id))
                    .Select(p => new PizzasForMenuViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        InitialPrice = p.InitialPrice,
                        ImageUrl = p.ImageUrl,
                        Description = p.Description,
                        DoughName = p.Dough.Name,
                        Products = p.PizzaProducts
                            .Select(pp => new ProductsForPizzaViewModel
                            {
                                Name = pp.Product.Name
                            })
                            .ToArray()
                    })
                    .ToArrayAsync();
        }

        public async Task<IEnumerable<PizzasForMenuViewModel>> GetAllPizzasAsync()
        {
            return await dbContext
                    .Pizzas
                    .Select(p => new PizzasForMenuViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        InitialPrice = p.InitialPrice,
                        ImageUrl = p.ImageUrl,
                        Description = p.Description,
                        DoughName = p.Dough.Name,
                        Products = p.PizzaProducts
                            .Select(pp => new ProductsForPizzaViewModel
                            {
                                Name = pp.Product.Name
                            })
                            .ToArray()
                    })
                    .ToArrayAsync();
        }

        public async Task<PizzaDetailsViewModel?> GetPizzaByIdAsync(int pizzaId)
        {
            Pizza? pizza = await dbContext.Pizzas
                .Include(p => p.PizzaProducts)
                    .ThenInclude(pp => pp.Product)
                .Include(p => p.Dough)
                .FirstOrDefaultAsync(p => p.Id == pizzaId);

            if (pizza == null)
            {
                return null;
            }

            PizzaDetailsViewModel viewModel = new PizzaDetailsViewModel
            {
                Id = pizza.Id,
                Name = pizza.Name,
                InitialPrice = pizza.InitialPrice,
                ImageUrl = pizza.ImageUrl,
                Description = pizza.Description,
                DoughName = pizza.Dough.Name,
                Products = pizza.PizzaProducts
                                .Select(pp => new ProductsForPizzaViewModel
                                {
                                    Name = pp.Product.Name
                                })
                                .ToArray()
            };

            return viewModel;
        }

        public async Task<EditPizzaViewModel?> GetPizzaForEditAsync(int id)
        {
            Pizza? pizzaToEdit = await dbContext
                .Pizzas
                .Include (p => p.PizzaProducts)
                .FirstOrDefaultAsync(p => p.Id == id);

            if(pizzaToEdit != null)
            {
                return new EditPizzaViewModel()
                {
                    Id = pizzaToEdit.Id,
                    Name = pizzaToEdit.Name,
                    InitialPrice = pizzaToEdit.InitialPrice,
                    ImageUrl = pizzaToEdit.ImageUrl,
                    Description = pizzaToEdit.Description,
                    DoughId = pizzaToEdit.DoughId,
                };
            }
            return null;
        }

        public async Task<IEnumerable<PizzasForMenuViewModel>> GetPizzasByMenuIdAsync(int menuId)
        {
            return await dbContext
                    .Pizzas
                    .Where(p => p.MenusPizzas.Any(mp => mp.MenuId == menuId))
                    .Select(p => new PizzasForMenuViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        InitialPrice = p.InitialPrice,
                        ImageUrl = p.ImageUrl,
                        Description = p.Description,
                        DoughName = p.Dough.Name,
                        Products = p.PizzaProducts
                            .Select(pp => new ProductsForPizzaViewModel
                            {
                                Name = pp.Product.Name
                            })
                            .ToArray()
                    })
                    .ToArrayAsync();
        }

        public async Task EditPizzaByIdAndEditModelAsync(int id, EditPizzaViewModel editModel)
        {
            Pizza pizza = await dbContext
                .Pizzas
                .FirstAsync(p => p.Id == id);

            if(pizza != null)
            {
                pizza.Name = editModel.Name;
                pizza.InitialPrice = editModel.InitialPrice;
                pizza.ImageUrl = editModel.ImageUrl;
                pizza.Description = editModel.Description;
                pizza.DoughId = editModel.DoughId;

                List<PizzaProduct> existingPizzaProducts = await dbContext.PizzasProducts
                    .Where(pp => pp.PizzaId == id)
                    .ToListAsync();

                dbContext.PizzasProducts.RemoveRange(existingPizzaProducts);

                if (editModel.ProductsId.Any())
                {
                    foreach (var productId in editModel.ProductsId)
                    {
                        PizzaProduct pp = new PizzaProduct()
                        {
                            PizzaId = id,
                            ProductId = productId
                        };

                        await this.dbContext.AddAsync(pp);
                    }
                }

                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task<DeletePizzaViewModel> GetPizzaForDeleteAsync(int id)
        {
            Pizza pizzaToDelete = await dbContext
                .Pizzas
                .FirstAsync(p => p.Id == id);

            return new DeletePizzaViewModel()
            {
                Name = pizzaToDelete.Name,
                InitialPrice = pizzaToDelete.InitialPrice,
                ImageUrl = pizzaToDelete.ImageUrl,
                Description = pizzaToDelete.Description
            };
        }

        public async Task DeleteByIdAsync(int id)
        {
            Pizza pizzaToDelete = await this.dbContext
                .Pizzas
                .FirstAsync(p => p.Id == id);

            this.dbContext.Remove(pizzaToDelete);
            this.dbContext.SaveChanges();

        }

        public async Task<OrderPizzaViewModel?> GetPizzaForOrderAsync(int pizzaId)
        {
            Pizza? pizza = await dbContext.Pizzas
                .Include(p => p.PizzaProducts)
                    .ThenInclude(pp => pp.Product)
                .Include(p => p.Dough)
                .Include(p => p.Toppings)
                .FirstOrDefaultAsync(p => p.Id == pizzaId);

            OrderPizzaViewModel viewModel = new OrderPizzaViewModel()
            {
                Id = pizza.Id,
                Name = pizza.Name,
                InitialPrice = pizza.InitialPrice,
                ImageUrl = pizza.ImageUrl,
                Description = pizza.Description,
                DoughName = pizza.Dough.Name,
                Products = pizza.PizzaProducts
                                .Select(pp => new ProductsForPizzaViewModel
                                {
                                    Name = pp.Product.Name
                                })
                                .ToArray(),
                Toppings = await toppingService.GetAllToppingsAsync()
            };

            return viewModel;
        }


    }
}
