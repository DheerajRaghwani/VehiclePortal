namespace VehiclePortal.QueryModel
{
    public class CheckpostDashboardQueryModel
    {
        public string District { get; set; } = null!;

        
        public string BlockName { get; set; } = null!;
        public int NoOfBuses { get; set; }
        public int TotalPeopleInBuses { get; set; }
        public int NoOfSmallVehicles { get; set; }
        public int TotalPeopleSmallVehicles { get; set; }
        public int TotalPeopleInDistrict { get; set; }
    }

    public class CheckpostDashboardResponseModel
    {
        public List<DistrictDashboardQueryModel> DistrictData { get; set; } = new();
        public int OverallPeople { get; set; }
    }
}

