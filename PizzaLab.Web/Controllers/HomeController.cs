using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaLab.Web.Models;
using System.Diagnostics;
using static PizzaLab.Common.GeneralApplicationConstants;

namespace PizzaLab.Web.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (this.User.IsInRole(AdminRoleName))
            {
                return this.RedirectToAction("Index", "Home", new { Area = AdminAreaName });
            }

            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("All", "Menu");
            }

            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 400 || statusCode == 404)
            {
                return View("Error404");
            }

            if (statusCode == 401)
            {
                return View("Error401");
            }

            return View();
        }
    }
}
