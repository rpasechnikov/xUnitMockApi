using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xUnitMockApi.Models;
using xUnitMockApi.Services.Interfaces;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly MockContext context;

        public VehicleService(MockContext context)
        {
            this.context = context;
        }

        public IEnumerable<VehicleViewModel> GetVehicles()
        {
            var viewModels = new List<VehicleViewModel>();

            foreach (var vehicleWithEngineAndWheels in context.Vehicles.Include(x => x.Engine).Include(x => x.Wheels))
            {
                viewModels.Add(new VehicleViewModel
                {
                    Name = vehicleWithEngineAndWheels.Name,
                    EngineId = vehicleWithEngineAndWheels.Engine?.EngineId,
                    WheelId = vehicleWithEngineAndWheels.Wheels?.FirstOrDefault()?.WheelId
                });
            }

            return viewModels;
        }

        public async Task<bool> CreateNewVehicle(VehicleViewModel vehicleVm)
        {
            try
            {
                var vehicle = new Vehicle
                {
                    Name = vehicleVm.Name
                };

                await context.Vehicles.AddAsync(vehicle);
                await context.SaveChangesAsync();
                
                if (vehicleVm.EngineId.HasValue)
                {
                    await context.VehicleEngines.AddAsync(new VehicleEngine
                    {
                        VehicleId = vehicle.Id,
                        EngineId = vehicleVm.EngineId.Value
                    });

                    await context.SaveChangesAsync();
                }

                if (vehicleVm.WheelId.HasValue)
                {
                    // Assumed 4 wheels
                    for (var i = 0; i < 4; i++)
                    {
                        await context.VehicleWheels.AddAsync(new VehicleWheel
                        {
                            VehicleId = vehicle.Id,
                            WheelId = vehicleVm.WheelId.Value
                        });
                    }

                    await context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                // TODO: handle ex
                return false;
            }
        }
    }
}
