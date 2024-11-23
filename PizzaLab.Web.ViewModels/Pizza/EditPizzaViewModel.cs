namespace PizzaLab.Web.ViewModels.Pizza
{
    using System.ComponentModel.DataAnnotations;

    using PizzaLab.Web.ViewModels.Dough;
    using PizzaLab.Web.ViewModels.Products;

    using static PizzaLab.Common.EntityValidationsConstants.Pizza;

    public class EditPizzaViewModel
    {
        public EditPizzaViewModel()
        {
            this.AvailableProducts = new HashSet<ProductsForPizzaViewModel>();
            this.ProductsId = new HashSet<int>();
            this.Doughs = new HashSet<DoughViewModel>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal InitialPrice { get; set; }

        [Required]
        [MinLength(ImageMinLength)]
        [MaxLength(ImageMaxLength)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [MinLength(DescriptionMinLength)]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public int DoughId { get; set; }

        public IEnumerable<DoughViewModel> Doughs { get; set; }

        public IEnumerable<ProductsForPizzaViewModel> AvailableProducts { get; set; }

        public IEnumerable<int> ProductsId { get; set; }

    }
}
