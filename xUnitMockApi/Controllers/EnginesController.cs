using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using xUnitMockApi.Services.Interfaces;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnginesController : ControllerBase
    {
        private readonly IEngineService engineService;

        public EnginesController(IEngineService engineService)
        {
            this.engineService = engineService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(engineService.GetEngines());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EngineViewModel engineViewModel)
        {
            if (!await engineService.CreateNewEngine(engineViewModel))
            {
                return BadRequest($"Unable to create engine for: {JsonConvert.SerializeObject(engineViewModel)}");
            }

            return NoContent();
        }
    }
}
