using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xUnitMockApi.Models;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly MockContext context;

        public VehiclesController(MockContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var viewModels = new List<VehicleViewModel>();

                foreach (var vehicleWithEngineAndWheels in context.Vehicles.Include(x => x.Engine)/*.Include(x => x.Wheels)*/)
                {
                    viewModels.Add(new VehicleViewModel
                    {
                        Name = vehicleWithEngineAndWheels.Name,
                        EngineId = vehicleWithEngineAndWheels.Engine.EngineId//,
                        //WheelId = vehicleWithEngineAndWheels.Wheels.FirstOrDefault().WheelId
                    });
                }

                return Ok(context.Vehicles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]VehicleViewModel vehicleViewModel)
        {
            try
            {
                var vehicle = new Vehicle
                {
                    Name = vehicleViewModel.Name
                };

                await context.Vehicles.AddAsync(vehicle);
                await context.SaveChangesAsync();
                
                if (vehicleViewModel.EngineId.HasValue)
                {
                    await context.VehicleEngines.AddAsync(new VehicleEngine
                    {
                        VehicleId = vehicle.Id,
                        EngineId = vehicleViewModel.EngineId.Value
                    });

                    await context.SaveChangesAsync();
                }

                if (vehicleViewModel.WheelId.HasValue)
                {
                    // Assumed 4 wheels
                    for (var i = 0; i < 4; i++)
                    {
                        await context.VehicleWheels.AddAsync(new VehicleWheel
                        {
                            VehicleId = vehicle.Id,
                            WheelId = vehicleViewModel.WheelId.Value
                        });
                    }

                    await context.SaveChangesAsync();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
