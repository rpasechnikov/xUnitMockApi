using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using xUnitMockApi.Extensions;
using xUnitMockApi.Helpers;
using xUnitMockApi.Models;
using xUnitMockApi.Services;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.IntegrationTests
{
    public class CreateAndGetVehiclesWithRelevantEntities
    {
        private readonly DbContextOptions<MockContext> emptyDbOptions =
            new DbContextOptionsBuilder<MockContext>()
                    .UseInMemoryDatabase(databaseName: "EmptyDb")
                    .Options;

        public const string VEHICLE_NAME = "VEHICLE";

        [Fact]
        public async Task ShouldCreateAndFetchNewVehiclesWithNoWheelsOrEnginesAsync()
        {
            using (var context = new MockContext(emptyDbOptions))
            {
                // Ensure DB was cleaned up
                context.ResetValueGenerators();
                await context.Database.EnsureDeletedAsync();

                var service = new VehicleService(context);

                var vehicleVm = new VehicleViewModel
                {
                    Name = VEHICLE_NAME + 1
                };

                // Ensure DB emtpy
                var noVehicles = service.GetVehicles();
                Assert.Empty(noVehicles);

                // Add new vehicle
                var isCreatedOk = await service.CreateNewVehicle(vehicleVm);
                Assert.True(isCreatedOk);

                // Ensre vehicle added OK
                var oneVehicles = service.GetVehicles();
                Assert.Single(oneVehicles);
            }
        }

        // https://andrewlock.net/creating-parameterised-tests-in-xunit-with-inlinedata-classdata-and-memberdata/
        [Theory]
        [MemberData(nameof(VehicleViewModelTestData))]
        public async Task ShouldCreateAndFetchNewVehiclesWithWheelsAndEnginesAsync(VehicleViewModel vehicleVm)
        {
            using (var context = new MockContext(DbHelper.GetNewDbOptions<MockContext>()))
            {
                // Ensure DB was cleaned up
                context.ResetValueGenerators();
                await context.Database.EnsureDeletedAsync();

                var vehicleService = new VehicleService(context);
                var wheelService = new WheelService(context);
                var engineService = new EngineService(context);

                var engineVm = new EngineViewModel
                {
                    Capacity = 4000,
                    Configuration = "V8",
                    FuelType = "Petrol"
                };

                var wheelVm = new WheelViewModel
                {
                    Size = 17,
                    Width = 7
                };

                // Ensure DB emtpy
                var noVehicles = vehicleService.GetVehicles();
                var noEngines = engineService.GetEngines();
                var noWheels = wheelService.GetWheels();

                Assert.Empty(noVehicles);
                Assert.Empty(noEngines);
                Assert.Empty(noWheels);

                // Add entities
                var isCreatedOk = await engineService.CreateNewEngine(engineVm);
                isCreatedOk = isCreatedOk && await wheelService.CreateNewWheel(wheelVm);

                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                // TODO: This should fail in the second execution due to foreign key constraint. We are trying to add a vehicle
                // with an engine and wheel combination that does not yet exist. This fails during normal execution.
                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                isCreatedOk = isCreatedOk && await vehicleService.CreateNewVehicle(vehicleVm);

                // Ensure all created ok
                Assert.True(isCreatedOk);

                // Ensure vehicle added OK
                var oneEngine = engineService.GetEngines();
                var oneWheel = wheelService.GetWheels();
                var oneVehicles = vehicleService.GetVehicles();

                Assert.Single(oneEngine);
                Assert.Single(oneWheel);
                Assert.Single(oneVehicles);

                // Verify many-to-many tables
                var oneVehicleEngine = context.VehicleEngines;
                var fourVehicleWheels = context.VehicleWheels;

                Assert.Single(oneVehicleEngine);
                Assert.Equal(4, fourVehicleWheels.Count());
            }
        }

        public static IEnumerable<object[]> VehicleViewModelTestData =>
            new List<object[]>
            {
                new object[] {new VehicleViewModel
                {
                    Name = VEHICLE_NAME + 1,
                    EngineId = 1,
                    WheelId = 1
                }},
                new object[] {new VehicleViewModel
                {
                    Name = VEHICLE_NAME + 2,
                    EngineId = 2,
                    WheelId = 2
                }},
                new object[] {new VehicleViewModel
                {
                    Name = VEHICLE_NAME + 3,
                    EngineId = 1,
                    WheelId = 1
                }}
            };
    }
}
