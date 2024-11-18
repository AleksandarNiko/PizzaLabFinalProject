using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static PizzaLab.Common.EntityValidationsConstants.Dough;

namespace PizzaLab.Data.Models
{
    public class Dough
    {
        public Dough()
        {
            this.Pizzas = new HashSet<Pizza>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public IEnumerable<Pizza> Pizzas { get; set; }
    }
}
