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
    public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(this.GenerateProducts());
        }

        private Product[] GenerateProducts()
        {
            ICollection<Product> products = new HashSet<Product>();

            Product product;

            product = new Product()
            {
                Id = 1,
                Name = "Tomato Sauce"
            };

            products.Add(product);

            product = new Product()
            {
                Id = 2,
                Name = "Cheese"
            };

            products.Add(product);

            product = new Product()
            {
                Id = 3,
                Name = "Pepperoni"
            };

            products.Add(product);

            product = new Product()
            {
                Id = 4,
                Name = "Vegetables"
            };

            products.Add(product);

            product = new Product()
            {
                Id = 5,
                Name = "Pineapple"
            };

            products.Add(product);

            return products.ToArray();
        }
    }
}
