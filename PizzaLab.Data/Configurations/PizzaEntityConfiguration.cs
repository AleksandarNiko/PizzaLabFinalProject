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
    public class PizzaEntityConfiguration : IEntityTypeConfiguration<Pizza>
    {
        public void Configure(EntityTypeBuilder<Pizza> builder)
        {
            builder
                .Property(p => p.InitialPrice)
                .HasColumnType("decimal(18, 2)");

            builder.HasData(this.GeneratePizzas());
        }

        private Pizza[] GeneratePizzas()
        {
            ICollection<Pizza> pizzas = new HashSet<Pizza>();

            Pizza pizza;

            pizza = new Pizza()
            {
                Id = 1,
                Name = "Margherita",
                InitialPrice = 10.99m,
                ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQSFF8PErjfcRq_lYAHhj2OrrqqTdY0FKohDA&usqp=CAU",
                Description = "Classic pizza with tomato sauce and mozzarella cheese",
                DoughId = 1
            };

            pizzas.Add(pizza);

            pizza = new Pizza()
            {
                Id = 2,
                Name = "Pepperoni",
                InitialPrice = 12.99m,
                ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT_4_lOV1P3_db6HITLwflwzROi6IZsHppD_g&usqp=CAU",
                Description = "Traditional pizza topped with tomato sauce and slices of pepperoni.",
                DoughId = 2
            };

            pizzas.Add(pizza);

            return pizzas.ToArray();
        }
    }
}
