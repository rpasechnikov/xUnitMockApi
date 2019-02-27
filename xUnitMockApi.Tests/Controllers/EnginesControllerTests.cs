using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;
using xUnitMockApi.Controllers;
using xUnitMockApi.Services.Interfaces;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.Tests.Controllers
{
    public class EnginesControllerTests
    {
        [Fact]
        public void ShouldBeConstructed()
        {
            var service = new Mock<IEngineService>();
            var controller = new EnginesController(service.Object);

            Assert.NotNull(controller);
        }

        [Fact]
        public void ShouldGet()
        {
            var service = new Mock<IEngineService>();
            var controller = new EnginesController(service.Object);

            var payload = new EngineViewModel[]
            {
                new EngineViewModel
                {
                    Capacity = 2400,
                    Configuration = "Inline 4 Vtec",
                    FuelType = "Petrol"
                }
            };

            service.Setup(x => x.GetEngines()).Returns(payload);

            var result = controller.Get();
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(((OkObjectResult)result).Value, payload);
        }

        [Fact]
        public async Task ShouldReturnBadRequestIfFailingToCreate()
        {
            var service = new Mock<IEngineService>();
            var controller = new EnginesController(service.Object);

            var viewModel = new EngineViewModel
            {
                Capacity = 2400,
                Configuration = "Inline 4 Vtec",
                FuelType = "Petrol"
            };

            service.Setup(x => x.CreateNewEngine(viewModel)).Returns(Task.FromResult(false));

            var result = await controller.Post(viewModel);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ShouldCreate()
        {
            var service = new Mock<IEngineService>();
            var controller = new EnginesController(service.Object);

            var viewModel = new EngineViewModel
            {
                Capacity = 2400,
                Configuration = "Inline 4 Vtec",
                FuelType = "Petrol"
            };

            service.Setup(x => x.CreateNewEngine(viewModel)).Returns(Task.FromResult(true));

            var result = await controller.Post(viewModel);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
