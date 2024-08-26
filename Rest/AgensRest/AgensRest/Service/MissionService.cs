using AgensRest.Models;
using AgentsApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgensRest.Service
{
    public class MissionService(ApplicationDBContext context,
       IServiceProvider serviceProvider
    ) : IMissionService
    {
        private IAgentService agentService = serviceProvider.GetRequiredService<IAgentService>();
        private ITargetService targetService = serviceProvider.GetRequiredService<ITargetService>();

        public async Task<List<MissionModel>> CreateListMissionsFromAgent(int agentId)
        {
            if (await agentService.IsAgentFree(agentId))
            {
                var agent = await agentService.FindAgentById(agentId);

                var validTargets1 = await context.Targets
                    .Where(t => Distance(agent, t) < 200).ToListAsync();
                var validTargets2 = await FilterValidTargetsAsync(validTargets1);
                List<MissionModel> missions = validTargets2.Select(t => new MissionModel
                {
                    AgentId = agent.Id,
                    TargetId = t.Id,
                }).ToList();

                return missions;

            }
            throw new Exception("no missions");
        }


        public double Distance(AgentModel agent, TargetModel target) =>
            Math.Sqrt(Math.Pow(target.X - agent.X, 2)
                + Math.Pow(target.Y - agent.Y, 2));


        public async Task<List<TargetModel>>
            FilterValidTargetsAsync(List<TargetModel> validTargets1)
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

        public async Task<MissionModel> CreateMission(MissionModel mission)
        {
            var agent = await agentService.FindAgentById(mission.AgentId);
            var target = await targetService.FindTargetById(mission.TargetId);
            var distance = Distance(agent, target);
            mission.RemainingTime = distance / 10 * 9;
            mission.Target = target;
            mission.Agent = agent;
            return mission;
        }
    }
}
