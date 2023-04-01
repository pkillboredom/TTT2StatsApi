using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TTT2StatsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<DamageLogController> _logger;
        private readonly IGmodSqliteService _gmodSqliteService;

        public PlayerController(ILogger<DamageLogController> logger, IServiceProvider services)
        {
            _logger = logger;
            _gmodSqliteService = services.GetRequiredService<IGmodSqliteService>();
        }


    }
}
