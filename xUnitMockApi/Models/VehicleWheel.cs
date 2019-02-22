namespace xUnitMockApi.Models
{
    public class VehicleWheel
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public int WheelId { get; set; }
        public Wheel Wheel { get; set; }
    }
}
