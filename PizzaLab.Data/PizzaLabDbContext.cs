using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzaLab.Data.Configurations;
using PizzaLab.Data.Models;

namespace PizzaLab.Data
{
    public class PizzaLabDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public PizzaLabDbContext(DbContextOptions<PizzaLabDbContext> options)
            : base(options)
        {

        }

        public DbSet<Cart> Carts { get; set; } = null!;

        public DbSet<Dough> Doughs { get; set; } = null!;

        public DbSet<Menu> Menus { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<Pizza> Pizzas { get; set; } = null!;

        public DbSet<Product> Product { get; set; } = null!;

        public DbSet<Topping> Toppings { get; set; } = null!;

        public DbSet<MenuPizza> MenusPizzas { get; set; } = null!;

        public DbSet<PizzaProduct> PizzasProducts { get; set; } = null!;

        public DbSet<CartPizza> CartsPizzas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityConfigurationHelper.ApplyEntityConfigurations(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
