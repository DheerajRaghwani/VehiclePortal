namespace VehiclePortal.QueryModel
{
    public class DashboardQueryModel
    {
        public int DistrictId { get; set; }
        public int BlockId { get; set; }
        public string DistrictName { get; set; }
        public string BlockName { get; set; }
        public int NumberOfBuses { get; set; }
        public int TotalBusCapacity { get; set; }
        public int NumberOfSmallVehicles { get; set; }
        public int TotalSmallVehicleCapacity { get; set; }
        public int TotalCapacityPerDistrict { get; set; }

    }

}

