using System.Collections.Generic;
using System.Threading.Tasks;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.Services.Interfaces
{
    public interface IWheelService
    {
        IEnumerable<WheelViewModel> GetWheels();
        Task<bool> CreateNewWheel(WheelViewModel vehicleVm);
    }
}
