using AgensRest.Models;
using AgensRest.Dto;
using AgentsApi.Data;

using Microsoft.EntityFrameworkCore;
using static AgensRest.Service.AgentService;

namespace AgensRest.Service
{
    public class AgentService(ApplicationDBContext context) : IAgentService
    {

        private readonly IAgentService _agentService;
        public async Task<List<AgentModel>> GetAllAgentsAsync() =>
            await context.Agents.ToListAsync();

        public async Task<AgentModel?> CreateAgentAsync(AgentDto agent)
        {
            AgentModel Agent = new AgentModel()
            {
                Image = agent.PhotoUrl,
                Nickname = agent.Nickname
            };
            await context.Agents.AddAsync(Agent);
            await context.SaveChangesAsync();
            return Agent;
        }
        

        public async Task<AgentModel?> FindAgentByIdAsync(int id) =>
            await context.Agents.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<AgentModel?> UpdateAgentAsync(int id, AgentModel agent)
        {
            AgentModel byId = await FindAgentByIdAsync(id)
                    ?? throw new Exception($"Agent by the id {id} doesnt exists");
            byId.Nickname = agent.Nickname;
            byId.Image = agent.Image;
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


