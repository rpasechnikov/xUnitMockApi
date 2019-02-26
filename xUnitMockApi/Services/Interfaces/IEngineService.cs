using System.Collections.Generic;
using System.Threading.Tasks;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.Services.Interfaces
{
    public interface IEngineService
    {
        IEnumerable<EngineViewModel> GetEngines();
        Task<bool> CreateNewEngine(EngineViewModel vehicleVm);
    }
}
