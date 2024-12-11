using Microsoft.AspNetCore.Mvc;
using PizzaLab.Services.Data.Interfaces;
using PizzaLab.Services.Data.Models;

namespace PizzaLab.WebApi.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IMenuService menuService;

        public StatisticsApiController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(StatisticsServiceModel))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                StatisticsServiceModel serviceModel =
                    await menuService.GetStatisticsAsync();

                return Ok(serviceModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
