using System.ComponentModel.DataAnnotations;

using static PizzaLab.Common.EntityValidationsConstants.Topping;
namespace PizzaLab.Data.Models
{
    public class Topping
    {
        public Topping()
        {
            this.Pizzas = new HashSet<Pizza>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        public ICollection<Pizza> Pizzas { get; set; }
    }
}
