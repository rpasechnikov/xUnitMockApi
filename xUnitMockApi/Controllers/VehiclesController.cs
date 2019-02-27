using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using xUnitMockApi.Services.Interfaces;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(vehicleService.GetVehicles());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]VehicleViewModel vehicleViewModel)
        {
            if (!await vehicleService.CreateNewVehicle(vehicleViewModel))
            {
                return BadRequest($"Unable to create a new vehicle for: {JsonConvert.SerializeObject(vehicleViewModel)}");
            }

            return NoContent();
        }
    }
}
