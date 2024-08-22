using AgensRest.Models;

namespace AgensRest.Service
{
    public interface ITargetService
    {
        Task<List<TargetModel>> GetAllTargetsAsync();
        Task<TargetModel?> FindTargetByIdAsync(int id);
        Task<TargetModel?> CreateTargetAsync(AgentModel agent);
        Task<TargetModel?> UpdateTargetAsync(int id, TargetModel target);
        Task<TargetModel?> DeleteTargetAsync(int id);

    }
}
