using AgensRest.Models;
using AgensRest.Service;
using Microsoft.AspNetCore.Mvc;
using AgensRest.Dto;

namespace AgensRest.Controllers
{
    [Route("")]
    [ApiController]
    public class AgentController(IAgentService agentService) : ControllerBase
    {
        [HttpGet("AllAgents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AgentModel>>> GetAll() =>
            Ok(await agentService.GetAllAgentsAsync());



        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AgentModel>> GetById(int id)
        {
            var agent = await agentService.FindAgentByIdAsync(id);
            return agent == null ? NotFound($"User by the id {id} dosent exists") : Ok(agent);
        }





        [HttpPost("agents")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AgentModel>> CreateUser([FromBody] AgentDto agent)
        {
            try
            {
                var Agent = await agentService.CreateAgentAsync(agent);
                return Created("", Agent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AgentModel>> Update(int id, [FromBody] AgentModel user)
        {
            try
            {
                var updated = await agentService.UpdateAgentAsync(id, user);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AgentModel>> Delete(int id)
        {
            try
            {
                return Ok(await agentService.DeleteAgentAsync(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
