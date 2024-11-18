using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaLab.Data.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public ApplicationUser User { get; set; } = null!;
    }
}
