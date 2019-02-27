using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;
using xUnitMockApi.Controllers;
using xUnitMockApi.Services.Interfaces;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.UnitTests.Controllers
{
    public class VehiclesControllerTests
    {
        [Fact]
        public void ShouldBeConstructed()
        {
            var service = new Mock<IVehicleService>();
            var controller = new VehiclesController(service.Object);

            Assert.NotNull(controller);
        }

        [Fact]
        public void ShouldGet()
        {
            var service = new Mock<IVehicleService>();
            var controller = new VehiclesController(service.Object);

            var payload = new VehicleViewModel[]
            {
                new VehicleViewModel
                {
                    Name = "Test Vehicle",
                    EngineId = 1,
                    WheelId = 1
                }
            };

            service.Setup(x => x.GetVehicles()).Returns(payload);

            var result = controller.Get();
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(((OkObjectResult)result).Value, payload);
        }

        [Fact]
        public async Task ShouldReturnBadRequestIfFailingToCreate()
        {
            var service = new Mock<IVehicleService>();
            var controller = new VehiclesController(service.Object);

            var viewModel = new VehicleViewModel
            {
                Name = "Test Vehicle",
                EngineId = 1,
                WheelId = 1
            };

            service.Setup(x => x.CreateNewVehicle(viewModel)).Returns(Task.FromResult(false));

            var result = await controller.Post(viewModel);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ShouldCreate()
        {
            var service = new Mock<IVehicleService>();
            var controller = new VehiclesController(service.Object);

            var viewModel = new VehicleViewModel
            {
                Name = "Test Vehicle",
                EngineId = 1,
                WheelId = 1
            };

            service.Setup(x => x.CreateNewVehicle(viewModel)).Returns(Task.FromResult(true));

            var result = await controller.Post(viewModel);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
