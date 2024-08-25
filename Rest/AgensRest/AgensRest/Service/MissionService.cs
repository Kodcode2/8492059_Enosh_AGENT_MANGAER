using AgensRest.Models;
using AgentsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace AgensRest.Service
{
    public class MissionService(ApplicationDBContext context,
       IServiceProvider serviceProvider
    ) : IMissionService
    {
       
    }
}
