using AgensRest.Dto;
using AgensRest.Models;
using AgentsApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgensRest.Service
{
    public class TargetService(ApplicationDBContext context) : ITargetService
    {
        private readonly Dictionary<string, (int, int)> Direction = new()
        {
            {"n", (0, 1)},
            {"s", (0, -1)},
            {"e", (-1, 0)},
            {"w", (1, 0)},
            {"ne", (-1, 1)},
            {"nw", (1, 1)},
            {"se", (-1, -1)},
            {"sw", (1, -1)}
        };
        public async Task<ActionResult<TargetModel>> DeleteTargetModelAsync(int id)
        {
            var target = await context.Targets.FindAsync(id);
            if (target == null)
            {
                return null;
            }

            context.Targets.Remove(target);
            await context.SaveChangesAsync();

            return target;
        }


        public async Task<List<TargetModel>> GetTargetsAsync()
        {
            return await context.Targets.ToListAsync();
        }

        public async Task<IdDto> CreateTargetModel(TargetDto targetDto)
        {
            try
            {
                TargetModel target = new()
                {
                    Image = targetDto.PhotoUrl,
                    Name = targetDto.Name,
                    Role = targetDto.Position
                };
                await context.Targets.AddAsync(target);
                await context.SaveChangesAsync();
                var a = await context.Targets.FirstOrDefaultAsync(A => A.Image == target.Image && A.Name == target.Name && A.Role == target.Role);
                IdDto idDto = new() { Id = a.Id };
                return idDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult<TargetModel>> UpdateTargetAsync(int id, TargetModel targetModel)
        {

            if (!context.Targets.Any(a => a.Id == id))
            {
                return null;
            }
            try
            {
                var target = await context.Targets.FirstOrDefaultAsync(a => a.Id == id);
                target.Name = targetModel.Name;
                target.Role = targetModel.Role;
                target.Image = targetModel.Image;
                await context.SaveChangesAsync();
                return target;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<TargetModel> Pin(PinDto pin, int id)
        {
            TargetModel? target = await context.Targets.FirstOrDefaultAsync(t => t.Id == id);
            if (target == null)
            {
                throw new Exception("Target not found");
            }
            target.X = pin.X;
            target.Y = pin.Y;
            await context.SaveChangesAsync();
            return target;
        }


        public async Task<TargetModel> FindTargetById(int id)
        {
            TargetModel? target = await context.Targets.FirstOrDefaultAsync(t => t.Id == id);
            if (target == null)
            {
                throw new Exception("Target not found");
            }
            return target;
        }
    }
}
