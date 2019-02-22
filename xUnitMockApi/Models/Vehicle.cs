using System.Collections.Generic;

namespace xUnitMockApi.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<VehicleWheel> Wheels { get; set; }
        public VehicleEngine Engine { get; set; }
    }
}
