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
    public class EngineServiceTests
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
                var service = new EngineService(context);
                Assert.NotNull(service);
            }
        }

        [Fact]
        public void ShouldGetNoEnginesWithEmptyDb()
        {
            using (var context = new MockContext(emptyDbOptions))
            {
                var service = new EngineService(context);
                Assert.Empty(service.GetEngines());
            }
        }

        [Fact]
        public async Task ShouldGetEngines()
        {
            var dbOptions = DbHelper.GetNewDbOptions<MockContext>();
            await CreateTestDatabase(dbOptions);

            using (var context = new MockContext(dbOptions))
            {
                var service = new EngineService(context);

                var results = service.GetEngines();
                var engine1 = results.First();
                var engine4 = results.Last();

                Assert.Equal(4, results.Count());

                Assert.Equal(2400, engine1.Capacity);
                Assert.Equal("Inline 4 Vtec", engine1.Configuration);
                Assert.Equal("Petrol", engine1.FuelType);

                Assert.Equal(2000, engine4.Capacity);
                Assert.Equal("Inline 4 Turbo", engine4.Configuration);
                Assert.Equal("Diesel", engine4.FuelType);
            }
        }

        [Fact]
        public async Task ShouldCreateEngineWithEmptyDb()
        {
            using (var context = new MockContext(DbHelper.GetNewDbOptions<MockContext>()))
            {
                var service = new EngineService(context);

                var engineVm = new EngineViewModel
                {
                    Capacity = 1600,
                    Configuration = "Inline 4",
                    FuelType = "Petrol"
                };

                var results = await service.CreateNewEngine(engineVm);
                var engine = context.Engines.First();

                Assert.Single(context.Engines);

                Assert.Equal(engineVm.Capacity, engine.Capacity);
                Assert.Equal(engineVm.Configuration, engine.Configuration);
                Assert.Equal(engineVm.FuelType, engine.FuelType);
            }
        }

        [Fact]
        public async Task ShouldCreateEngineWithExistingDb()
        {
            var dbOptions = DbHelper.GetNewDbOptions<MockContext>();
            await CreateTestDatabase(dbOptions);

            using (var context = new MockContext(dbOptions))
            {
                var service = new EngineService(context);

                var engineVm = new EngineViewModel
                {
                    Capacity = 1600,
                    Configuration = "Inline 4",
                    FuelType = "Petrol"
                };

                var results = service.CreateNewEngine(engineVm);
                var engines = context.Engines;
                var engine1 = engines.First();
                var engine5 = engines.Last();

                Assert.Equal(5, engines.Count());

                Assert.Equal(2400, engine1.Capacity);
                Assert.Equal("Inline 4 Vtec", engine1.Configuration);
                Assert.Equal("Petrol", engine1.FuelType);

                Assert.Equal(engineVm.Capacity, engine5.Capacity);
                Assert.Equal(engineVm.Configuration, engine5.Configuration);
                Assert.Equal(engineVm.FuelType, engine5.FuelType);
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

                await context.Engines.AddRangeAsync(engines);
                await context.SaveChangesAsync();
            }
        }
    }
}
