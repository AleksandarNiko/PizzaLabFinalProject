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
    public class DoughEntityConfiguration : IEntityTypeConfiguration<Dough>
    {
        public void Configure(EntityTypeBuilder<Dough> builder)
        {
            builder.HasData(this.GenerateDoughs());
        }
        private Dough[] GenerateDoughs()
        {
            ICollection<Dough> doughs = new HashSet<Dough>();

            Dough dough;

            dough = new Dough()
            {
                Id = 1,
                Name = "Classic Neapolitan"
            };
            doughs.Add(dough);

            dough = new Dough()
            {
                Id = 2,
                Name = "Whole Wheat"
            };
            doughs.Add(dough);

            dough = new Dough()
            {
                Id = 3,
                Name = "Gluten-Free"
            };
            doughs.Add(dough);

            return doughs.ToArray();
        }
    }
}
