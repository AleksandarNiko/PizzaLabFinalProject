namespace PizzaLab.Web.ViewModels.Cart
{
    public class CartItemViewModel
    {
        public int PizzaId { get; set; }

        public decimal Price { get; set; }

        public Guid UserId { get; set; }

        public string PizzaName { get; set; } = null!;

        public int CartId { get; set; }
    }
}
