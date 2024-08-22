using AgensRest.Models;

namespace AgensRest.Service
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetAllMissionsAsync();
        Task<MissionModel?> FindMissionByIdAsync(int id);
        Task<MissionModel?> CreateMissionAsync(MissionModel mission);
        Task<MissionModel?> UpdateMissionAsync(int id, MissionModel mission);
        Task<MissionModel?> DeleteMissionAsync(int id);
    }
}
