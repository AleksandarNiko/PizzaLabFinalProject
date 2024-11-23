namespace PizzaLab.Web.ViewModels.Pizza
{
    using PizzaLab.Web.ViewModels.Products;

    public class PizzaDetailsViewModel
    {
        public PizzaDetailsViewModel()
        {
            this.Products = new HashSet<ProductsForPizzaViewModel>();
        }
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal InitialPrice { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string DoughName { get; set; } = null!;

        public IEnumerable<ProductsForPizzaViewModel> Products { get; set; }
    }
}
