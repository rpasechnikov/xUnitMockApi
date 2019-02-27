# xUnitMockApi
Mock dotnet core API with a bunch of unit and a few integration tests to try and reproduce/demonstrate some of the EF core in-memory DB provider issues that I have been experiencing.

## Current issues:
* xUnitMockApi.IntegrationTests.ShouldCreateAndFetchNewVehiclesWithWheelsAndEnginesAsync:ln.99
** EF seems to be not evaluating relationships, until manually resolving relevant entities
** Hack/workaround to simply get relevant entities resolves the issue
* xUnitMockApi.UnitTests.Services.VehicleServiceTests:ln.125
** Foreign key constraints seem to not be honoured while in-memory db testing
