namespace PizzaLab.Web.ViewModels.Pizza
{
    public class DeletePizzaViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal InitialPrice { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
