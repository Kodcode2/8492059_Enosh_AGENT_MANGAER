using AgensRest.Models;
using AgentsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace AgensRest.Service
{
    public class TargetService(ApplicationDBContext context) : ITargetService
    {

        private readonly ITargetService _targetService;
        public async Task<List<TargetModel>> GetAllTargetsAsync() =>
            await context.Targets.ToListAsync();

        public async Task<TargetModel?> CreateAgentAsync(TargetModel target)
        {
            if (await FindTargetByIdAsync(target.Id) != null)
            {
                throw new Exception($"Agent by the Id {target.Id} is already exists");
            }
            target.Id = target.Id;
            await context.Targets.AddAsync(target);
            await context.SaveChangesAsync();
            return target;
        }

        public async Task<TargetModel?> FindTargetByIdAsync(int id) =>
            await context.Targets.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<TargetModel?> UpdateTargetAsync(int id, TargetModel target)
        {
            TargetModel byId = await FindTargetByIdAsync(id)
                    ?? throw new Exception($"Agent by the id {id} doesnt exists");
            byId.Name = target.Name;
            byId.Role = target.Role;
            byId.Image = target.Image;
            await context.SaveChangesAsync();
            return byId;
        }

        public async Task<TargetModel?> DeleteTargetAsync(int id)
        {
            TargetModel byId = await FindTargetByIdAsync(id)
                        ?? throw new Exception($"Target by the id {id} doesnt exists");
            context.Targets.Remove(byId);
            await context.SaveChangesAsync();
            return byId;
        }

        public Task<TargetModel?> CreateTargetAsync(AgentModel agent)
        {
            throw new NotImplementedException();
        }

    }
}


