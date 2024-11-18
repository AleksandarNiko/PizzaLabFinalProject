using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PizzaLab.Common.EntityValidationsConstants.Menu;

namespace PizzaLab.Data.Models
{
    public class Menu
    {
        public Menu()
        {
            this.MenusPizzas = new HashSet<MenuPizza>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public ICollection<MenuPizza>? MenusPizzas { get; set; }
    }
}
