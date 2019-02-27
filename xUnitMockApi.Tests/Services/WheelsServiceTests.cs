using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using xUnitMockApi.Extensions;
using xUnitMockApi.Helpers;
using xUnitMockApi.Models;
using xUnitMockApi.Services;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.Tests.Services
{
    public class WheelsServiceTests
    {
        private readonly DbContextOptions<MockContext> emptyDbOptions =
            new DbContextOptionsBuilder<MockContext>()
                    .UseInMemoryDatabase(databaseName: "EmptyDb")
                    .Options;

        [Fact]
        public void ShouldBeConstructed()
        {
            using (var context = new MockContext(emptyDbOptions))
            {
                var service = new WheelService(context);
                Assert.NotNull(service);
            }
        }

        [Fact]
        public void ShouldGetNoWheelsWithEmptyDb()
        {
            using (var context = new MockContext(emptyDbOptions))
            {
                var service = new WheelService(context);
                Assert.Empty(service.GetWheels());
            }
        }

        [Fact]
        public async Task ShouldGetWheels()
        {
            var dbOptions = DbHelper.GetNewDbOptions<MockContext>();
            await CreateTestDatabase(dbOptions);

            using (var context = new MockContext(dbOptions))
            {
                var service = new WheelService(context);

                var results = service.GetWheels();
                var wheel1 = results.First();
                var wheel4 = results.Last();

                Assert.Equal(4, results.Count());

                Assert.Equal(15, wheel1.Size);
                Assert.Equal(5, wheel1.Width);

                Assert.Equal(18, wheel4.Size);
                Assert.Equal(7, wheel4.Width);
            }
        }

        [Fact]
        public async Task ShouldCreateWheelWithEmptyDb()
        {
            using (var context = new MockContext(DbHelper.GetNewDbOptions<MockContext>()))
            {
                var service = new WheelService(context);

                var wheelVm = new WheelViewModel
                {
                    Size = 19,
                    Width = 7
                };

                var isCreateSuccess = await service.CreateNewWheel(wheelVm);
                var wheel = context.Wheels.First();

                Assert.True(isCreateSuccess);
                Assert.Single(context.Wheels);

                Assert.Equal(wheelVm.Size, wheel.Size);
                Assert.Equal(wheelVm.Width, wheel.Width);
            }
        }

        [Fact]
        public async Task ShouldCreateWheelWithExistingDb()
        {
            var dbOptions = DbHelper.GetNewDbOptions<MockContext>();
            await CreateTestDatabase(dbOptions);

            using (var context = new MockContext(dbOptions))
            {
                var service = new WheelService(context);

                var wheelVm = new WheelViewModel
                {
                    Size = 19,
                    Width = 7
                };

                var isCreateSuccess = await service.CreateNewWheel(wheelVm);
                var wheels = context.Wheels;
                var wheel1 = wheels.First();
                var wheel5 = wheels.Last();

                Assert.True(isCreateSuccess);
                Assert.Equal(5, wheels.Count());

                Assert.Equal(15, wheel1.Size);
                Assert.Equal(5, wheel1.Width);

                Assert.Equal(wheelVm.Size, wheel5.Size);
                Assert.Equal(wheelVm.Width, wheel5.Width);
            }
        }

        /// <summary>
        /// Creates the test DB, resetting indices, unless data is already present
        /// </summary>
        /// <param name="dbOptions"></param>
        /// <returns></returns>
        private static async Task CreateTestDatabase(DbContextOptions<MockContext> dbOptions)
        {
            using (var context = new MockContext(dbOptions))
            {
                // Ensure DB was cleaned up
                context.ResetValueGenerators();
                await context.Database.EnsureDeletedAsync();

                var wheels = new Wheel[]
                {
                    new Wheel
                    {
                        Size = 15,
                        Width = 5
                    },
                    new Wheel
                    {
                        Size = 16,
                        Width = 5
                    },
                    new Wheel
                    {
                        Size = 17,
                        Width = 6
                    },
                    new Wheel
                    {
                        Size = 18,
                        Width = 7
                    }
                };

                await context.Wheels.AddRangeAsync(wheels);
                await context.SaveChangesAsync();
            }
        }
    }
}
