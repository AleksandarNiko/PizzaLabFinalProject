namespace PizzaLab.Web.ViewModels.Pizza
{
    using PizzaLab.Web.ViewModels.Products;
    using PizzaLab.Web.ViewModels.Topping;

    public class OrderPizzaViewModel
    {
        public OrderPizzaViewModel()
        {
            this.Products = new HashSet<ProductsForPizzaViewModel>();
            this.Toppings = new HashSet<ToppingForPizzaVIewModel>();
            this.SelectedToppingIds = new HashSet<int>();
        }
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal InitialPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string DoughName { get; set; } = null!;

        public IEnumerable<ProductsForPizzaViewModel> Products { get; set; }

        public IEnumerable<ToppingForPizzaVIewModel> Toppings { get; set; }

        public IEnumerable<int> SelectedToppingIds { get; set; }
    }
}
