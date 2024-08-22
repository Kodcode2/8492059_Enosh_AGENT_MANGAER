using AgensRest.Models;
using AgentsApi.Data;
using Microsoft.EntityFrameworkCore;
using static AgensRest.Service.AgentService;

namespace AgensRest.Service
{
    public class AgentService(ApplicationDbContext context) : IAgentService
    {

        private readonly IAgentService _agentService;
        public async Task<List<AgentModel>> GetAllAgentsAsync() =>
            await context.Agents.ToListAsync();

        public async Task<AgentModel?> CreateAgentAsync(AgentModel agent)
        {
            if (await FindAgentByIdAsync(agent.Id) != null)
            {
                throw new Exception($"Agent by the Id {agent.Id} is already exists");
            }
            agent.Id = agent.Id;    
            await context.Agents.AddAsync(agent);
            await context.SaveChangesAsync();
            return agent;
        }

        public async Task<AgentModel?> FindAgentByIdAsync(int id) =>
            await context.Agents.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<AgentModel?> UpdateAgentAsync(int id, AgentModel agent)
        {
            AgentModel byId = await FindAgentByIdAsync(id)
                    ?? throw new Exception($"Agent by the id {id} doesnt exists");
            byId.Nickname = agent.Nickname;
            byId.PhotoUrl = agent.PhotoUrl;
            await context.SaveChangesAsync();
            return byId;
        }

        public async Task<AgentModel?> DeleteAgentAsync(int id)
        {
            AgentModel byId = await FindAgentByIdAsync(id)
                    ?? throw new Exception($"Agent by the id {id} doesnt exists");
            context.Agents.Remove(byId);
            await context.SaveChangesAsync();
            return byId;
        }

     
    }
}


