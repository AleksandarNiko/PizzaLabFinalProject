using Microsoft.AspNetCore.Mvc;

namespace PizzaLab.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
