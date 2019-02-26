﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using xUnitMockApi.Models;
using xUnitMockApi.Services.Interfaces;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WheelsController : ControllerBase
    {
        private readonly MockContext context;
        private readonly IWheelService wheelService;

        public WheelsController(MockContext context, IWheelService wheelService)
        {
            this.context = context;
            this.wheelService = wheelService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(wheelService.GetWheels());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]WheelViewModel wheelViewModel)
        {
            if (!await wheelService.CreateNewWheel(wheelViewModel))
            {
                return BadRequest($"Unable to create wheel for: {JsonConvert.SerializeObject(wheelViewModel)}");
            }

            return NoContent();
        }
    }
}
