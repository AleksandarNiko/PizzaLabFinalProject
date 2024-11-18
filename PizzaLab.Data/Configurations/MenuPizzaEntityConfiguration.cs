using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaLab.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaLab.Data.Configurations
{
    public class MenuPizzaEntityConfiguration : IEntityTypeConfiguration<MenuPizza>
    {
        public void Configure(EntityTypeBuilder<MenuPizza> builder)
        {
            builder.HasKey(pp => new { pp.PizzaId, pp.MenuId });

            builder.HasOne(pp => pp.Pizza)
                .WithMany(p => p.MenusPizzas)
                .HasForeignKey(pp => pp.PizzaId);

            builder.HasOne(pp => pp.Menu)
                .WithMany(p => p.MenusPizzas)
                .HasForeignKey(pp => pp.MenuId);

            builder.HasData(this.GenerateMenuPizzas());
        }

        private MenuPizza[] GenerateMenuPizzas()
        {
            ICollection<MenuPizza> menuPizzas = new HashSet<MenuPizza>();

            MenuPizza menuPizza;

            menuPizza = new MenuPizza()
            {
                PizzaId = 1,
                MenuId = 1,
            };

            menuPizzas.Add(menuPizza);

            menuPizza = new MenuPizza()
            {
                PizzaId = 2,
                MenuId = 1,
            };

            menuPizzas.Add(menuPizza);

            return menuPizzas.ToArray();
        }
    }
}
