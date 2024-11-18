using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaLab.Data.Models
{
    public class PizzaProduct
    {
        [ForeignKey(nameof(Pizza))]
        public int? PizzaId { get; set; }
        public Pizza? Pizza { get; set; }


        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
