namespace PizzaLab.Web.ViewModels.Pizza
{
    using PizzaLab.Web.ViewModels.Dough;
    using PizzaLab.Web.ViewModels.Products;
    using System.ComponentModel.DataAnnotations;

    using static PizzaLab.Common.EntityValidationsConstants.Pizza;

    public class AddPizzaViewModel
    {
        public AddPizzaViewModel()
        {
            this.AvailableProducts = new HashSet<ProductsForPizzaViewModel>();
            this.Doughs = new HashSet<DoughViewModel>();
            this.ProductsId = new HashSet<int>();
        }

        [Required]
        [MinLength(NameMinLength)]
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
