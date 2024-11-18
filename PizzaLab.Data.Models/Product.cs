using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PizzaLab.Common.EntityValidationsConstants.Product;
namespace PizzaLab.Data.Models
{
    public class Product
    {
        public Product()
        {
            this.PizzaProducts = new HashSet<PizzaProduct>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<PizzaProduct> PizzaProducts { get; set; }
    }
}
