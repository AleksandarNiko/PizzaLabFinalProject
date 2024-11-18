using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaLab.Data.Models
{
    public class CartPizza
    {
        [ForeignKey(nameof(Cart))]
        public int CartId { get; set; }
        public Cart Cart { get; set; }


        [ForeignKey(nameof(Pizza))]
        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; }

        public decimal UpdatedPrice { get; set; }
    }
}
