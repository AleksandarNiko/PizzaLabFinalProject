namespace PizzaLab.Web.ViewModels.Cart
{
    public class ViewCartViewModel
    {
        public ViewCartViewModel()
        {
            this.Items = new List<CartItemViewModel>();
        }
        public decimal FinalPrize { get; set; }
        public List<CartItemViewModel> Items { get; set; }
    }
}
