using AgensRest.Models;
using AgensRest.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AgensRest.Service
{
    public interface IAgentService
    {
        Task<List<AgentModel>> GetAgentsAsync();
        Task<IdDto> CreateAgentModel(AgentDto agentDto);
        Task<ActionResult<AgentModel>> UpdateAgentAsync(int id, AgentModel agentModel);
        Task<ActionResult<AgentModel>> DeleteAgentModelAsync(int id);
        Task<AgentModel> Pin(PinDto pin, int id);
        Task<AgentModel> FindAgentById(int id);
    }
}
