namespace PizzaLab.Web.ViewModels.Menu
{
    using System.ComponentModel.DataAnnotations;
    using PizzaRestaurant.Web.ViewModels.Pizza;

    using static PizzaLab.Common.EntityValidationsConstants.Menu;

    public class EditMenuViewModel
    {
        public EditMenuViewModel()
        {
            this.MenuPizzas = new HashSet<PizzasForMenuViewModel>();
            this.SelectedPizzas = new List<int>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        [MinLength(NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; } = null!;

        public IEnumerable<PizzasForMenuViewModel>? MenuPizzas { get; set; }

        public List<int> SelectedPizzas { get; set; }
    }
}
