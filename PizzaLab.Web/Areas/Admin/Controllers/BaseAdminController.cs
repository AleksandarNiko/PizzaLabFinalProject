using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static PizzaLab.Common.GeneralApplicationConstants;
namespace PizzaLab.Web.Areas.Admin.Controllers
{
    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class BaseAdminController : Controller
    {

    }
}
