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
            [HttpGet("AllMission")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            public async Task<ActionResult<List<MissionModel>>> GetAll() =>
                Ok(await missionService.GetAllMissionsAsync());


            [HttpGet("{id}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<MissionModel>> GetById(int id)
            {
                var agent = await missionService.FindMissionByIdAsync(id);
                return agent == null ? NotFound($"User by the id {id} dosent exists") : Ok(agent);
            }


            [HttpPost("create")]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<MissionModel>> CreateUser([FromBody] MissionModel model)
            {
                try
                {
                    var agent = await missionService.CreateMissionAsync(model);
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
            public async Task<ActionResult<MissionModel>> Update(int id, [FromBody] MissionModel target)
            {
                try
                {
                    var updated = await missionService.UpdateMissionAsync(id, target);
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
                    return Ok(await missionService.DeleteMissionAsync(id));
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }

        }
    }
