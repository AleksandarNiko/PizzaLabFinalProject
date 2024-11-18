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
    public class MenuEntityConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasData(this.GenerateProducts());
        }

        private Menu[] GenerateProducts()
        {
            ICollection<Menu> menus = new HashSet<Menu>();

            Menu menu;

            menu = new Menu()
            {
                Id = 1,
                Name = "Breakfast menu",
                Description = "Nice menu"
            };

            menus.Add(menu);

            return menus.ToArray();
        }
    }
}
