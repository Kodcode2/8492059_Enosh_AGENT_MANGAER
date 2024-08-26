using AgensRest.Models;
using AgensRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgensRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionController(IMissionService missionService) : ControllerBase
    {
        [HttpPost("update")]
        public async Task<ActionResult> Create(MissionModel mission)
        {
            try
            {
                await missionService.CreateMission(mission);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
