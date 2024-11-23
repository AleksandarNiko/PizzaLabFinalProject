namespace PizzaLab.Web.ViewModels.Menu
{
    using System.ComponentModel.DataAnnotations;

    using static PizzaLab.Common.EntityValidationsConstants.Menu;

    public class DeleteMenuViewModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;
    }
}
