using AgensRest.Models;
using AgensRest.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AgensRest.Service
{
    public interface IAgentService
    {
        Task<List<AgentModel>> GetAgentsAsync();
        Task<ActionResult<AgentModel>> GetAgentModelAsync(int id);
        Task<IdDto> CreateAgentModel(AgentDto agentDto);
        Task<ActionResult<AgentModel>> UpdateAgentAsync(int id, AgentModel agentModel);
        Task<ActionResult<AgentModel>> DeleteAgentModelAsync(int id);
        Task<AgentModel> MoveAgent(int id, DirectionsDto directionDto);
        Task<AgentModel> Pin(PinDto pin, int id);
        Task<bool> IsAgentFree(int id);
        Task<AgentModel> FindAgentById(int id);
    }
}
