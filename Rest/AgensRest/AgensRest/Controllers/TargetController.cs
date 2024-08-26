using AgensRest.Dto;
using AgensRest.Models;
using AgensRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgensRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TargetsController(ITargetService targetService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<TargetModel>>> GetTarget()
        {
            return Ok(await targetService.GetTargetsAsync());
        }

        [HttpGet("get-target/{id}")]
        public async Task<ActionResult<TargetModel>> GetTargetModel(int id)
        {
            try
            {
                return Ok(await targetService.GetTargetModelAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-target/{id}")]
        public async Task<ActionResult<TargetModel>> PutTargetModel(int id, [FromBody] TargetModel target)
        {
            try
            {
                var a = await targetService.UpdateTargetAsync(id, target);
                return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] TargetDto targetDto)
        {
            try
            {
                var t = await targetService.CreateTargetModel(targetDto);
                return Created("success", t);
            }
            catch (Exception ex)
            {
                var x = 0;
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-target/{id}")]
        public async Task<IActionResult> DeleteTargetModel(int id)
        {
            try
            {
                await targetService.DeleteTargetModelAsync(id);
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
                return Ok(await targetService.Pin(pinDto, id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/move")]
        public async Task<ActionResult<TargetModel>>
            MoveAsync(DirectionsDto directions, int id)
        {
            try
            {
                return Ok(await targetService.MoveTarget(id, directions));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
