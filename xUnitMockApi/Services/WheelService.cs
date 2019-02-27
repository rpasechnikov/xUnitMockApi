using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xUnitMockApi.Models;
using xUnitMockApi.Services.Interfaces;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.Services
{
    public class WheelService : IWheelService
    {
        private readonly MockContext context;

        public WheelService(MockContext context)
        {
            this.context = context;
        }

        public IEnumerable<WheelViewModel> GetWheels()
        {
            var viewModels = new List<WheelViewModel>();

            foreach (var wheel in context.Wheels)
            {
                viewModels.Add(new WheelViewModel
                {
                    Size = wheel.Size,
                    Width = wheel.Width
                });
            }

            return viewModels;
        }

        public async Task<bool> CreateNewWheel(WheelViewModel wheelVm)
        {
            try
            {
                var wheel = new Wheel
                {
                    Size = wheelVm.Size,
                    Width = wheelVm.Width
                };

                await context.Wheels.AddAsync(wheel);
                await context.SaveChangesAsync();
                
                return true;
            }
            catch
            {
                // TODO: handle ex
                return false;
            }
        }
    }
}
