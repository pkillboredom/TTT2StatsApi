using Microsoft.AspNetCore.Mvc;
using TTT2StatsApi.Models;

namespace TTT2StatsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoundController : ControllerBase
    {
        private readonly ILogger<DamageLogController> _logger;
        private readonly IGmodSqliteService _gmodSqliteService;

        public RoundController(ILogger<DamageLogController> logger, IServiceProvider services)
        {
            _logger = logger;
            _gmodSqliteService = services.GetRequiredService<IGmodSqliteService>();
        }

        [HttpGet(Name = "GetRounds")]
        public ActionResult<IEnumerable<RoundRow>> Get()
        {
            try
            {
                return Ok(_gmodSqliteService.GetRounds());
            }
            catch(Exception ex)
            {
                return Problem(title: ex.Message, detail: ex.StackTrace);
            }
        }

        // GET: Round/5
        [HttpGet("{id}", Name = "GetRoundById")]
        public ActionResult<RoundRow?> Get(int id)
        {
            try
            {
                RoundRow? value = _gmodSqliteService.GetRoundById(id);
                if (value == null)
                {
                    return NotFound();
                }
                return Ok(value);
            }
            catch(Exception ex)
            {
                return Problem(title: ex.Message, detail: ex.StackTrace);
            }
        }

        // GET: Round/5/Players
        [HttpGet("{id}/Players", Name = "GetPlayerInfosByRoundId")]
        public ActionResult<IEnumerable<PlayerRoundInfoRow>> GetPlayerInfosByRoundId(int id)
        {
            try
            {
                return Ok(_gmodSqliteService.GetPlayerRoundInfos(id));
            }
            catch(Exception ex)
            {
                return Problem(title: ex.Message, detail: ex.StackTrace);
            }
        }

        // GET: Round/5/DeathsByPlayer/76561198043854879
        [HttpGet("{id}/DeathsByPlayer/{steamId}", Name = "GetPlayerKillsDeathsByRoundIdAndSteamId")]
        public ActionResult<IEnumerable<PlayerDeathRow>> GetPlayerKillsDeathsByRoundIdAndSteamId(int id, string steamId)
        {
            try
            {
                return Ok(_gmodSqliteService.GetPlayerKillsDeathsByRoundIdAndSteamId(id, steamId));
            }
            catch(Exception ex)
            {
                return Problem(title: ex.Message, detail: ex.StackTrace);
            }
        }

        [HttpGet("{map}", Name = "GetRoundsByMap")]
        public ActionResult<IEnumerable<RoundRow>> GetRoundsByMap(string map)
        {
            try
            {
                return Ok(_gmodSqliteService.GetRoundsByMap(map));
            }
            catch(Exception ex)
            {
                return Problem(title: ex.Message, detail: ex.StackTrace);
            }
        }

        [HttpGet("mapcounts", Name = "GetMapCounts")]
        public ActionResult<IEnumerable<MapCountRow>> GetMapCounts()
        {
            try
            {
                return Ok(_gmodSqliteService.GetMapCounts());
            }
            catch(Exception ex)
            {
                return Problem(title: ex.Message, detail: ex.StackTrace);
            }
        }
    }
}
