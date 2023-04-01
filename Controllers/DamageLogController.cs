using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTT2StatsApi.Models;

namespace TTT2StatsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DamageLogController : ControllerBase
    {
        private readonly ILogger<DamageLogController> _logger;
        private readonly IGmodSqliteService _gmodSqliteService;
        public DamageLogController(ILogger<DamageLogController> logger, IServiceProvider services)
        {
            _logger = logger;
            _gmodSqliteService = services.GetRequiredService<IGmodSqliteService>();
        }

        [HttpGet(Name = "GetDamageLog")]
        public ActionResult<IEnumerable<CombatLogRow>> Get()
        {
            try
            {
                return Ok(_gmodSqliteService.GetCombatLog());
            }
            catch(Exception ex)
            {
                return Problem(title: ex.Message, detail: ex.StackTrace);
            }
        }
    }
}
