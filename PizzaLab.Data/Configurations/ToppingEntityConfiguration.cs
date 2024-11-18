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
    public class ToppingEntityConfiguration : IEntityTypeConfiguration<Topping>
    {
        public void Configure(EntityTypeBuilder<Topping> builder)
        {
            builder
              .Property(t => t.Price)
              .HasColumnType("decimal(18, 2)");

            builder.HasData(this.GenerateToppings());
        }

        private Topping[] GenerateToppings()
        {
            ICollection<Topping> toppings = new HashSet<Topping>();

            Topping topping;

            topping = new Topping()
            {
                Id = 1,
                Price = 1.20m,
                Name = "Cheese"
            };

            toppings.Add(topping);

            topping = new Topping()
            {
                Id = 2,
                Price = 1.30m,
                Name = "Mushrooms"
            };

            toppings.Add(topping);

            topping = new Topping()
            {
                Id = 3,
                Price = 1.00m,
                Name = "Olives"
            };

            toppings.Add(topping);

            topping = new Topping()
            {
                Id = 4,
                Price = 1.50m,
                Name = "Garlic"
            };

            toppings.Add(topping);

            return toppings.ToArray();
        }
    }
}
