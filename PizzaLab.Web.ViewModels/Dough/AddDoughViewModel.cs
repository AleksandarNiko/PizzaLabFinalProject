namespace PizzaLab.Web.ViewModels.Dough
{
    using System.ComponentModel.DataAnnotations;

    using static PizzaLab.Common.EntityValidationsConstants.Dough;

    public class AddDoughViewModel
        {
        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

    }
}
