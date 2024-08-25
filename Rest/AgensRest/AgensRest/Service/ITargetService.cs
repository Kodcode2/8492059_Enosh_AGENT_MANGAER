using AgensRest.Dto;
using AgensRest.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgensRest.Service
{
    public interface ITargetService
    {
        Task<List<TargetModel>> GetTargetsAsync();
        Task<IdDto> CreateTargetModel(TargetDto targetDto);
        Task<ActionResult<TargetModel>> UpdateTargetAsync(int id, TargetModel targetModel);
        Task<ActionResult<TargetModel>> DeleteTargetModelAsync(int id);
        Task<TargetModel> Pin(PinDto pin, int id);
        Task<TargetModel> FindTargetById(int id);
    }
}
