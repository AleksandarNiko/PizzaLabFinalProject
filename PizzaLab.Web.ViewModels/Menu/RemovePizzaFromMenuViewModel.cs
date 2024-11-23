namespace PizzaLab.Web.ViewModels.Menu
{
    public class RemovePizzaFromMenuViewModel
    {
        public int MenuId { get; set; }
        public int PizzaId { get; set; }
        public string MenuName { get; set; } = null!;
        public string PizzaName { get; set; } = null!;
    }
}
