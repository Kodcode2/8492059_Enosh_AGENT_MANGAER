using AgensRest.Models;
using AgentsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace AgensRest.Service
{
    public class MissionService(ApplicationDbContext context) : IMissionService
    {
            private readonly IMissionService _missionService;
            public async Task<List<MissionModel>> GetAllMissionsAsync() =>
                await context.Missions.ToListAsync();

            public async Task<MissionModel?> CreateMissionAsync(MissionModel mission)
            {
                if (await FindMissionByIdAsync(mission.Id) != null)
                {
                    throw new Exception($"Mission by the Id {mission.Id} is already exists");
                }
                mission.Id = mission.Id;
                await context.Missions.AddAsync(mission);
                await context.SaveChangesAsync();
                return mission;
            }

            public async Task<MissionModel?> FindMissionByIdAsync(int id) =>
                await context.Missions.FirstOrDefaultAsync(u => u.Id == id);

            public async Task<MissionModel?> UpdateMissionAsync(int id, MissionModel mission)
            {
                MissionModel byId = await FindMissionByIdAsync(id)
                        ?? throw new Exception($"Mission by the id {id} doesnt exists");
                byId.Agent = mission.Agent;
                byId.TimeRemaining = mission.TimeRemaining;
                await context.SaveChangesAsync();
                return byId;
            }

            public async Task<MissionModel?> DeleteMissionAsync(int id)
            {
            MissionModel byId = await FindMissionByIdAsync(id)
                        ?? throw new Exception($"Agent by the id {id} doesnt exists");
                context.Missions.Remove(byId);
                await context.SaveChangesAsync();
                return byId;
            }


        }
    }

