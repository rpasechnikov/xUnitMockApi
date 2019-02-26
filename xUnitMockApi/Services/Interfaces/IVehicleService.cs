using System.Collections.Generic;
using System.Threading.Tasks;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.Services.Interfaces
{
    public interface IVehicleService
    {
        IEnumerable<VehicleViewModel> GetVehicles();
        Task<bool> CreateNewVehicle(VehicleViewModel vehicleVm);
    }
}
