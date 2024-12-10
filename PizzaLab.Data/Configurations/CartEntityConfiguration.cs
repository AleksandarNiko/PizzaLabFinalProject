using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaLab.Data.Models;

namespace PizzaLab.Data.Configurations
{
    public class CartEntityConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder
               .Property(c => c.FinalPrice)
               .HasColumnType("decimal(18, 2)");
        }
    }
}
