using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xUnitMockApi.Models;
using xUnitMockApi.Services.Interfaces;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.Services
{
    public class EngineService : IEngineService
    {
        private readonly MockContext context;

        public EngineService(MockContext context)
        {
            this.context = context;
        }

        public IEnumerable<EngineViewModel> GetEngines()
        {
            var viewModels = new List<EngineViewModel>();

            foreach (var engine in context.Engines)
            {
                viewModels.Add(new EngineViewModel
                {
                    Capacity = engine.Capacity,
                    FuelType = engine.FuelType,
                    Configuration = engine.Configuration
                });
            }

            return viewModels;
        }

        public async Task<bool> CreateNewEngine(EngineViewModel engineVm)
        {
            try
            {
                var engine = new Engine
                {
                    Capacity = engineVm.Capacity,
                    FuelType = engineVm.FuelType,
                    Configuration = engineVm.Configuration
                };

                await context.Engines.AddAsync(engine);
                await context.SaveChangesAsync();
                
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
