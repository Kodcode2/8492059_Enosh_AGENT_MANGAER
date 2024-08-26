using AgensRest.Models;

namespace AgensRest.Service
{
    public interface IMissionService
    {
        Task<List<MissionModel>> CreateListMissionsFromAgent(int id);
        Task<MissionModel> CreateMission(MissionModel mission);


    }
}
