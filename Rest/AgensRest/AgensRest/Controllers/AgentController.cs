using AgensRest.Models;
using AgensRest.Service;
using Microsoft.AspNetCore.Mvc;
using AgensRest.Dto;

namespace AgensRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AgentsController(IAgentService _agentService) : ControllerBase
    {



        [HttpPut("update-agent/{id}")]
        public async Task<IActionResult> PutAgentModel(int id, AgentModel agent)
        {
            try
            {
                await _agentService.UpdateAgentAsync(id, agent);
                return Ok(agent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<IdDto>> PostAgentModel([FromBody] AgentDto agentDto)
        {
            try
            {

                return Created("success", await _agentService.CreateAgentModel(agentDto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-agent/{id}")]
        public async Task<IActionResult> DeleteAgentModel(int id)
        {
            try
            {
                await _agentService.DeleteAgentModelAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/pin")]
        public async Task<ActionResult<TargetModel>> PinAsync(PinDto pinDto, int id)
        {
            try
            {
                return Ok(await _agentService.Pin(pinDto, id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

