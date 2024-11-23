namespace PizzaLab.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using static PizzaLab.Common.EntityValidationsConstants.Product;

    public class AddProductViewModel
    {
        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }
    }
}
