namespace VehiclePortal.QueryModel
{
    public class DistrictDashboardQueryModel
    {
        public string District { get; set; } = null!;
        public string BlockName { get; set; } = null!;

        public int NoOfBuses { get; set; }
        public int TotalPeopleInBuses { get; set; }
        public int NoOfSmallVehicles { get; set; }
        public int TotalPeopleSmallVehicles { get; set; }
        public int TotalPeopleInDistrict { get; set; }
    }

    public class DashboardResponseModel
    {
        public List<DistrictDashboardQueryModel> DistrictData { get; set; } = new();
        public int OverallPeople { get; set; }
    }
}

