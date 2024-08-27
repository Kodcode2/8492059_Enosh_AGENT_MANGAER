using AgensRest.Models;
using AgentTargetRest.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgentTargetRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MissionsController(IMissionService missionService) : ControllerBase
    {

        [HttpPost("update")]
        public async Task UpdateAsync()
        {
            await missionService.MainUpdate();
        }

        [HttpPut("{id}")]
        public async Task FuncAsync(int id)
        {
            await missionService.MainMissionFuncAsync(id);
        }

        [HttpGet]
        public async Task<ActionResult<List<MissionModel>>> GetAllMissionsAsync() =>
            await missionService.GetAllMisionsAsync();

        [HttpPost("create-missions{agentId}")]
        public async Task<ActionResult<List<MissionModel>>> CreateMission(int agentId)
        {
            try
            {
                return Ok(await missionService.CreateListMissionsFromAgentPinMoveAsync(agentId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-all")]
        public async Task DeleteMissions()
        {
            await missionService.Delete();
        }

        [HttpGet("get-all")]
        public async Task<ActionResult> GetAllMissions()
        {
            try
            {
                return Ok(await missionService.GetAllMisionsAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-propose")]
        public async Task<ActionResult> GetProposeMissions()
        {
            try
            {
                return Ok(await missionService.GetProposeMisionsAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}