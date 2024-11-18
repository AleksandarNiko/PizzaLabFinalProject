using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaLab.Data.Models
{
    public class Cart
    {
        public Cart()
        {
            this.CartsPizzas = new HashSet<CartPizza>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public ICollection<CartPizza> CartsPizzas { get; set; }

        [Required]
        public decimal FinalPrice { get; set; }
    }
}
