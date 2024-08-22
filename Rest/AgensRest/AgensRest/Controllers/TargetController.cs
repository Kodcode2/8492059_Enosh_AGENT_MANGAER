using AgensRest.Models;
using AgensRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgensRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetController(ITargetService targetService) : ControllerBase
    {
        [HttpGet("AllTargets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AgentModel>>> GetAll() =>
            Ok(await targetService.GetAllTargetsAsync());



        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AgentModel>> GetById(int id)
        {
            var agent = await targetService.FindTargetByIdAsync(id);
            return agent == null ? NotFound($"User by the id {id} dosent exists") : Ok(agent);
        }





        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AgentModel>> CreateUser([FromBody] AgentModel model)
        {
            try
            {
                var agent = await targetService.CreateTargetAsync(model);
                return Created("", agent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TargetModel>> Update(int id, [FromBody] TargetModel target)
        {
            try
            {
                var updated = await targetService.UpdateTargetAsync(id, target);
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
                return Ok(await targetService.DeleteTargetAsync(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}