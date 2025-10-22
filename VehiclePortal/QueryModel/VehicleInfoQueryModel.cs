namespace VehiclePortal.QueryModel
{
    public class VehicleInfoQueryModel
    {
        public string VehicleNo { get; set; } = null!;
        public string DistrictName { get; set; } = null!;
        public string VehicleType { get; set; } = null!;
        public int SeatCapacity { get; set; }
    }
}
