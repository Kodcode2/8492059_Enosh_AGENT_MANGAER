using AgensRest.Dto;
using AgensRest.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgensRest.Service
{
    public interface ITargetService
    {
        Task<List<TargetModel>> GetTargetsAsync();
        Task<ActionResult<TargetModel>> GetTargetModelAsync(int id);
        Task<IdDto> CreateTargetModel(TargetDto targetDto);
        Task<ActionResult<TargetModel>> UpdateTargetAsync(int id, TargetModel targetModel);
        Task<ActionResult<TargetModel>> DeleteTargetModelAsync(int id);
        Task<TargetModel> MoveTarget(int id, DirectionsDto directionDto);
        Task<TargetModel> Pin(PinDto pin, int id);
        Task<bool> IsTargetValid(TargetModel target);
        Task<TargetModel> FindTargetById(int id);
    }
}
