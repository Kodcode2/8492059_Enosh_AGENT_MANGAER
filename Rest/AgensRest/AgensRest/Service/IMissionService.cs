using AgensRest.Models;

namespace AgentTargetRest.Services
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetAllMisionsAsync();
        Task<List<MissionModel>> GetProposeMisionsAsync();
        Task<List<MissionModel>> GetOnTaskMisionsAsync();
        Task<List<MissionModel>> GetEndedMisionsAsync();

        Task<List<MissionModel>> CreateListMissionsFromAgentPinMoveAsync(int agentId);
        Task<List<MissionModel>> CreateListMissionsFromTargetPinMoveAsync(int targetId);
        Task MainMissionFuncAsync(int missionId);
        Task MainUpdate();
        Task Delete();
    }
}