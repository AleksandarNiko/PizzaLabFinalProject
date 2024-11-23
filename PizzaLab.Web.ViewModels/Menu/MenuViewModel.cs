namespace PizzaLab.Web.ViewModels.Menu
{
    using System.ComponentModel.DataAnnotations;

    public class MenuViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;
    }
}
