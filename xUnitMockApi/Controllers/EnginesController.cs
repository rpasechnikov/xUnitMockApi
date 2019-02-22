using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using xUnitMockApi.Models;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnginesController : ControllerBase
    {
        private readonly MockContext context;

        public EnginesController(MockContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // TODO: VMs
                return Ok(context.Engines);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EngineViewModel vehicleViewModel)
        {
            try
            {
                await context.Engines.AddAsync(new Engine
                {
                    Configuration = vehicleViewModel.Configuration,
                    FuelType = vehicleViewModel.FuelType,
                    Capacity = vehicleViewModel.Capacity
                });

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
