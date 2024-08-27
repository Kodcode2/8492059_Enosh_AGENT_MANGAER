using AgensRest.Models;
using AgensRest.Service;
using AgentsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace AgentTargetRest.Services
{
    public class MissionService(ApplicationDBContext context,
       IServiceProvider serviceProvider
    ) : IMissionService
    {
        private IAgentService agentService => serviceProvider.GetRequiredService<IAgentService>();
        private ITargetService targetService => serviceProvider.GetRequiredService<ITargetService>();

        public async Task<List<MissionModel>> GetAllMisionsAsync() =>
            await context.Missions.ToListAsync();
        public async Task<List<MissionModel>> GetProposeMisionsAsync() =>
            await context.Missions.Where(m => m.Status == MissionStatus.Proposal).ToListAsync();
        public async Task<List<MissionModel>> GetOnTaskMisionsAsync() =>
            await context.Missions.Where(m => m.Status == MissionStatus.Active).ToListAsync();
        public async Task<List<MissionModel>> GetEndedMisionsAsync() =>
            await context.Missions.Where(m => m.Status == MissionStatus.Proposal).ToListAsync();




        public async Task<List<TargetModel>> FilterValidTargetsAsync(List<TargetModel> validTargets1)
        {
            var validTargets2 = new List<TargetModel>();

            var tasks = validTargets1.Select(async t =>
            {
                var isValid = await targetService.IsTargetValid(t);
                return new { Target = t, IsValid = isValid };
            });

            var results = await Task.WhenAll(tasks);
            validTargets2 = results.Where(r => r.IsValid).Select(r => r.Target).ToList();

            return validTargets2;
        }



        public async Task<List<MissionModel>> CreateListMissionsFromAgentPinMoveAsync(int agentId)
        {
            if (await agentService.IsAgentFree(agentId))
            {
                var agent = await agentService.FindAgentById(agentId);
                var missions = await context.Missions.ToListAsync();
                var targets = await context.Targets.ToListAsync();
                var validTargetsByDistance = await context.Targets.Where(t => (Math.Sqrt(Math.Pow(t.X - agent.X, 2)
              + Math.Pow(t.Y - agent.Y, 2))) < 200).ToListAsync();
                List<TargetModel> targetsWithoutMissions = targets.Where(t => !missions.Exists(m => m.TargetId == t.Id)).ToList();
                List<TargetModel> validTargets = [
                    ..targetsWithoutMissions,
                    ..(from m in missions
                    join t in validTargetsByDistance on m.TargetId equals t.Id
                    where m.Status == MissionStatus.Proposal &&
                    m.AgentId != agentId &&
                    m.TargetId != t.Id
                    select t)
                .ToList()
                ];


                List<MissionModel> createMissions = validTargets.Select(t => new MissionModel
                {
                    AgentId = agent.Id,
                    TargetId = t.Id,
                    RemainingTime = Distance(agent, t) / 5,
                })
                    .Where(m => !missions.Exists(em => em.TargetId != m.TargetId && em.AgentId != m.AgentId))
                    .ToList();

                await context.Missions.AddRangeAsync(createMissions);
                await context.SaveChangesAsync();
                return missions;
            }
            throw new Exception("The agent on task in another mission");
        }


        //public async Task<List<MissionModel>> CreateListMissionsFromTargetPinMoveAsync(int targetId)
        //{
        //    var target = await targetService.FindTargetById(targetId);
        //    // Check if there is no other agent on the target
        //    if (context.Missions.Any(m => m.TargetId == targetId && m.Status == MissionStatus.Proposal) || !context.Missions.Any(m => m.TargetId == targetId))
        //    {
        //        if (target.Status == TargetStatus.Alive)
        //        {
        //            // Filter the agent if their are too far
        //            var validAgents1 = await context.Agents.Where(a => (Math.Sqrt(Math.Pow(target.X - a.X, 2)
        //            + Math.Pow(target.Y - a.Y, 2))) < 200).ToListAsync();
        //            var validAgents2 = validAgents1.Where(agentService.IsAgentValid);
        //            // create list of missions after the filters
        //            var missions = await context.Missions.ToListAsync();
        //            List<MissionModel> missionsToSave = validAgents2
        //                .Select(a => new MissionModel
        //                {
        //                    AgentId = a.Id,
        //                    TargetId = targetId,
        //                    RemainingTime = Distance(a, target) / 5,
        //                })
        //            .Where(m => !missions.Exists(em => em.TargetId != m.TargetId && em.AgentId != m.AgentId))
        //            .ToList();
        //            context.Missions.AddRange(missions);
        //            context.SaveChanges();
        //            return missionsToSave;
        //        }
        //        throw new Exception("There is already agent on this target");
        //    }
        //    throw new Exception("The target is is dead:)");
        //}


        public bool IsStatusesAvalableToAssinge
            (MissionModel mission, TargetModel target, AgentModel agent)
        {
            if (
                 mission.Status == MissionStatus.Proposal ||
                  target.Status == TargetStatus.Alive ||
                  agent.Status == AgentStatus.Dormant
              ) return true; return false;
        }


        public async Task ChangeStatusesAfterAssingeAsync
            (MissionModel mission, TargetModel target, AgentModel agent)
        {
            await CalculateTimeUntilAssassinAndMoveTheAgentAsync(agent, target!, mission);
            mission.Status = MissionStatus.Active;
            agent.Status = AgentStatus.Active;
            mission.Target = target;
            mission.Agent = agent;
            var distance = Distance(agent, target);
            mission.RemainingTime = distance / 5;

            await context.SaveChangesAsync();
        }

        public void DeleteMission(MissionModel mission)
        {
            context.Missions.Remove(mission);
        }

        private bool RmoveableMissions(MissionModel mission, AgentModel agent, TargetModel target) => (
            mission.Status == MissionStatus.Proposal
            && mission.TargetId == target.Id || mission.AgentId == agent.Id
        );


        public async Task<MissionModel> FilterMissionsAndDeleteAfterAssigned
        (MissionModel mission, AgentModel agent, TargetModel target)
        {
            try
            {
                if (mission.Status == MissionStatus.Proposal
                    && mission.TargetId == target.Id || mission.AgentId == agent.Id)
                {
                    DeleteMission(mission);
                    await context.SaveChangesAsync();
                }
                return mission;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task UpdateMissionUserAsync(MissionModel mission, AgentModel agent, TargetModel target)
        {
            var distance = Distance(agent, target);

            if (distance > 200)
            {
                context.Missions.Remove(mission);
                throw new Exception("The target went far");
            }
            await ChangeStatusesAfterAssingeAsync(mission, target, agent);
            var missions = await context.Missions.Where(m => m.Status == 0).ToListAsync();

            var removeableMissions = missions.Where(m => RmoveableMissions(m, agent, target)).ToList();
            context.Missions.RemoveRange(removeableMissions);
            await context.SaveChangesAsync();

            await CalculateTimeUntilAssassinAndMoveTheAgentAsync(agent, target, mission);

        }

        public double Distance(AgentModel agent, TargetModel target) =>
          Math.Sqrt(Math.Pow(target.X - agent.X, 2)
              + Math.Pow(target.Y - agent.Y, 2));

        public (int x, int y) MoveAgentAfterTarget1(AgentModel agent, TargetModel target)
        {
            (int x, int y) agentLocation = (agent.X, agent.Y);

            if (agentLocation.x < target.X) agentLocation.x++;
            else if (agentLocation.x > target.X) agentLocation.x--;

            if (agentLocation.y < target.Y) agentLocation.y++;
            else if (agentLocation.y > target.Y) agentLocation.y--;

            return (agentLocation.x == agent.X && agentLocation.y == agent.Y) ? (-10, -10) : agentLocation;
        }

        public async Task CalculateTimeUntilAssassinAndMoveTheAgentAsync
            (AgentModel agent, TargetModel target, MissionModel mission)
        {
            (int x, int y) agentLocation = MoveAgentAfterTarget1(agent!, target!);
            if (agentLocation.x == -10 && agentLocation.y == -10)
            {

                AfterTheMissionSuccess(mission, agent, target);
                return;
            }
            agent!.X = agentLocation.x;
            agent.Y = agentLocation.y;
            var distance = Distance(agent, target);
            mission.RemainingTime = distance / 5;
            await context.SaveChangesAsync();
        }

        public void AfterTheMissionSuccess(MissionModel mission, AgentModel agent, TargetModel target)
        {
            mission.Status = MissionStatus.Proposal;
            agent.Status = AgentStatus.Dormant;
            target.Status = TargetStatus.Hunted;
            context.SaveChanges();
        }

        public async Task MainMissionFuncAsync(int missionId)
        {
            var mission = await context.Missions.FirstOrDefaultAsync(m => m.Id == missionId);
            if (mission != null)
            {
                var target = await context.Targets.FirstOrDefaultAsync(t => t.Id == mission.TargetId);
                var agent = await context.Agents.FirstOrDefaultAsync(a => a.Id == mission.AgentId);
                if (agent == null || target == null) { throw new Exception("Problem with agent or the target"); }
                if (IsStatusesAvalableToAssinge(mission, target, agent))
                {
                    await UpdateMissionUserAsync(mission, agent, target);
                    return;
                }
            }
        }

        public async Task MainUpdate()
        {
            var missions = await context.Missions
                .Include(m => m.Agent)
                .Include(m => m.Target)
                .Where(m => m.Status == MissionStatus.Active)
                .ToListAsync();
            var tasks = missions.Select(async (m) => await CalculateTimeUntilAssassinAndMoveTheAgentAsync(m.Agent!, m.Target!, m)).ToArray();
            Task.WaitAll(tasks);
        }

        public async Task Delete()
        {
            var z = context.Missions.Select(x => x);
            context.Missions.RemoveRange(z);
            await context.SaveChangesAsync();
        }

        public Task<List<MissionModel>> CreateListMissionsFromTargetPinMoveAsync(int targetId)
        {
            throw new NotImplementedException();
        }

    }
}
