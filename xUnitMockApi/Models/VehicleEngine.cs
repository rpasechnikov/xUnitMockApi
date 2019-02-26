namespace xUnitMockApi.Models
{
    public class VehicleEngine
    {
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public int EngineId { get; set; }
        public Engine Engine { get; set; }
    }
}
