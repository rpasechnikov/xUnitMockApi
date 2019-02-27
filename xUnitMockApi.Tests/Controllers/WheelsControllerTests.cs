using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;
using xUnitMockApi.Controllers;
using xUnitMockApi.Services.Interfaces;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.Tests.Controllers
{
    public class WheelsControllerTests
    {
        [Fact]
        public void ShouldBeConstructed()
        {
            var service = new Mock<IWheelService>();
            var controller = new WheelsController(service.Object);

            Assert.NotNull(controller);
        }

        [Fact]
        public void ShouldGet()
        {
            var service = new Mock<IWheelService>();
            var controller = new WheelsController(service.Object);

            var payload = new WheelViewModel[]
            {
                new WheelViewModel
                {
                    Size = 16,
                    Width = 5
                }
            };

            service.Setup(x => x.GetWheels()).Returns(payload);

            var result = controller.Get();
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(((OkObjectResult)result).Value, payload);
        }

        [Fact]
        public async Task ShouldReturnBadRequestIfFailingToCreate()
        {
            var service = new Mock<IWheelService>();
            var controller = new WheelsController(service.Object);

            var viewModel = new WheelViewModel
            {
                Size = 16,
                Width = 5
            };

            service.Setup(x => x.CreateNewWheel(viewModel)).Returns(Task.FromResult(false));

            var result = await controller.Post(viewModel);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ShouldCreate()
        {
            var service = new Mock<IWheelService>();
            var controller = new WheelsController(service.Object);

            var viewModel = new WheelViewModel
            {
                Size = 16,
                Width = 5
            };

            service.Setup(x => x.CreateNewWheel(viewModel)).Returns(Task.FromResult(true));

            var result = await controller.Post(viewModel);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
