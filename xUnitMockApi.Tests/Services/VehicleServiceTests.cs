using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using xUnitMockApi.Extensions;
using xUnitMockApi.Helpers;
using xUnitMockApi.Models;
using xUnitMockApi.Services;
using xUnitMockApi.ViewModels;

namespace xUnitMockApi.UnitTests.Services
{
    public class VehicleServiceTests
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
                var service = new VehicleService(context);
                Assert.NotNull(service);
            }
        }

        [Fact]
        public void ShouldGetNoVehiclesWithEmptyDb()
        {
            using (var context = new MockContext(emptyDbOptions))
            {
                var service = new VehicleService(context);
                Assert.Empty(service.GetVehicles());
            }
        }

        [Fact]
        public async Task ShouldGetVehicles()
        {
            var dbOptions = DbHelper.GetNewDbOptions<MockContext>();
            await CreateTestDatabase(dbOptions);

            using (var context = new MockContext(dbOptions))
            {
                var service = new VehicleService(context);

                var results = service.GetVehicles();
                var vehicle1 = results.First();
                var vehicle4 = results.Last();

                Assert.Equal(4, results.Count());

                Assert.Equal("Subaru Impreza", vehicle1.Name);
                Assert.Equal(2, vehicle1.EngineId);
                Assert.Equal(2, vehicle1.WheelId);

                Assert.Equal("Honda Accord", vehicle4.Name);
                Assert.Equal(1, vehicle4.EngineId);
                Assert.Equal(3, vehicle4.WheelId);
            }
        }

        [Fact]
        public async Task ShouldCreateVehicleWithEmptyDb()
        {
            using (var context = new MockContext(DbHelper.GetNewDbOptions<MockContext>()))
            {
                var service = new VehicleService(context);

                var vehicleVm = new VehicleViewModel
                {
                    Name = "Test Vehicle"
                };

                var isCreateSuccess = await service.CreateNewVehicle(vehicleVm);
                var vehicle = context.Vehicles.First();

                Assert.True(isCreateSuccess);
                Assert.Single(context.Vehicles);

                Assert.Equal(vehicleVm.Name, vehicle.Name);
            }
        }

        [Fact]
        public async Task ShouldCreateVehicleWithExistingDb()
        {
            var dbOptions = DbHelper.GetNewDbOptions<MockContext>();
            await CreateTestDatabase(dbOptions);

            using (var context = new MockContext(dbOptions))
            {
                var service = new VehicleService(context);

                var vehicleVm = new VehicleViewModel
                {
                    Name = "Test Vehicle",
                    EngineId = 1,
                    WheelId = 1
                };

                var isCreateSuccess = await service.CreateNewVehicle(vehicleVm);
                var vehicles = context.Vehicles;
                var vehicle1 = vehicles.First();
                var vehicle5 = vehicles.Last();

                Assert.True(isCreateSuccess);
                Assert.Equal(5, vehicles.Count());

                // At this point vehicle1.Wheels and vehicle1.Engine are null. 
                // Inspecting the context.VehicleWheels and context.VehicleEngines verifies that these 
                // actually exist and then inspecting vehicle1.Wheels and vehicle1.Engine are no longer null
                //
                // TL;DR: properties are null, until inspected in context, after which they are no longer null
                // 
                // HACK: evaluating vehicleWheels and vehicleEngines manually results in these properties not being null as well
                // Is this expected? Unit test passes/succeeds based on context inspection, etc. Changes not propogating correctly...
                //var wheels = context.VehicleWheels.ToArray();
                //var engines = context.VehicleEngines.ToArray();
                Assert.Equal("Subaru Impreza", vehicle1.Name);
                Assert.Equal(2, vehicle1.Engine.EngineId);
                Assert.Equal(2, vehicle1.Wheels.First().WheelId);

                Assert.Equal("Test Vehicle", vehicle5.Name);
                Assert.Equal(1, vehicle5.Engine.EngineId);
                Assert.True(vehicle5.Wheels.All(x => x.WheelId == 1));
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

                // Wheels
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

                // Engines
                var engines = new Engine[]
                {
                    new Engine
                    {
                        Capacity = 2400,
                        Configuration = "Inline 4 Vtec",
                        FuelType = "Petrol"
                    },
                    new Engine
                    {
                        Capacity = 2000,
                        Configuration = "Boxer 4",
                        FuelType = "Petrol"
                    },
                    new Engine
                    {
                        Capacity = 4000,
                        Configuration = "V8",
                        FuelType = "Petrol"
                    },
                    new Engine
                    {
                        Capacity = 2000,
                        Configuration = "Inline 4 Turbo",
                        FuelType = "Diesel"
                    }
                };

                // Vehicles
                var vehicles = new Vehicle[]
                {
                    new Vehicle
                    {
                        Name = "Subaru Impreza"
                    },
                    new Vehicle
                    {
                        Name = "Volkswagen Caddy"
                    },
                    new Vehicle
                    {
                        Name = "Holden Commodore"
                    },
                    new Vehicle
                    {
                        Name = "Honda Accord"
                    }
                };

                await context.Wheels.AddRangeAsync(wheels);
                await context.Engines.AddRangeAsync(engines);
                await context.Vehicles.AddRangeAsync(vehicles);
                await context.SaveChangesAsync();

                // VehicleEngines
                var vehicleEngines = new VehicleEngine[]
                {
                    new VehicleEngine
                    {
                        VehicleId = 1,
                        EngineId = 2
                    },
                    new VehicleEngine
                    {
                        VehicleId = 2,
                        EngineId = 4
                    },
                    new VehicleEngine
                    {
                        VehicleId = 3,
                        EngineId = 3
                    },
                    new VehicleEngine
                    {
                        VehicleId = 4,
                        EngineId = 1
                    },
                };

                // VehicleWheels
                var vehicleWheels = new VehicleWheel[]
                {
                    // Subaru Impreza
                    new VehicleWheel
                    {
                        VehicleId = 1,
                        WheelId = 2
                    },
                    new VehicleWheel
                    {
                        VehicleId = 1,
                        WheelId = 2
                    },
                    new VehicleWheel
                    {
                        VehicleId = 1,
                        WheelId = 2
                    },
                    new VehicleWheel
                    {
                        VehicleId = 1,
                        WheelId = 2
                    },

                    // VW Caddy
                    new VehicleWheel
                    {
                        VehicleId = 2,
                        WheelId = 1
                    },
                    new VehicleWheel
                    {
                        VehicleId = 2,
                        WheelId = 1
                    },
                    new VehicleWheel
                    {
                        VehicleId = 2,
                        WheelId = 1
                    },
                    new VehicleWheel
                    {
                        VehicleId = 2,
                        WheelId = 1
                    },

                    // Holden Commodore
                    new VehicleWheel
                    {
                        VehicleId = 3,
                        WheelId = 4
                    },
                    new VehicleWheel
                    {
                        VehicleId = 3,
                        WheelId = 4
                    },
                    new VehicleWheel
                    {
                        VehicleId = 3,
                        WheelId = 4
                    },
                    new VehicleWheel
                    {
                        VehicleId = 3,
                        WheelId = 4
                    },

                    // Honda Accord
                    new VehicleWheel
                    {
                        VehicleId = 4,
                        WheelId = 3
                    },
                    new VehicleWheel
                    {
                        VehicleId = 4,
                        WheelId = 3
                    },
                    new VehicleWheel
                    {
                        VehicleId = 4,
                        WheelId = 3
                    },
                    new VehicleWheel
                    {
                        VehicleId = 4,
                        WheelId = 3
                    }
                };

                await context.VehicleEngines.AddRangeAsync(vehicleEngines);
                await context.VehicleWheels.AddRangeAsync(vehicleWheels);
                await context.SaveChangesAsync();
            }
        }
    }
}
