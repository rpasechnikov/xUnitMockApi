namespace xUnitMockApi.Models
{
    public class Engine
    {
        public int Id { get; set; }
        public string Configuration { get; set; }
        public string FuelType { get; set; }

        /// <summary>
        /// Capacity in CC
        /// </summary>
        public int Capacity { get; set; }
    }
}
