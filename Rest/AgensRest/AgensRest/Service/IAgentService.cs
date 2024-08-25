using AgensRest.Models;
using AgensRest.Dto;

namespace AgensRest.Service
{
    public interface IAgentService
    {
        Task<List<AgentModel>> GetAllAgentsAsync();
        Task<AgentModel?> FindAgentByIdAsync(int id);
        Task<AgentModel?> CreateAgentAsync(AgentDto agent);
        Task<AgentModel?> UpdateAgentAsync(int id, AgentModel agent);
        Task<AgentModel?> DeleteAgentAsync(int id);
    }
}
