namespace VehiclePortal.CommandModel
{
    public class VehicleregistrationCommandModel
    {
        public string VehicleNo { get; set; } = null!;

        public string VehicleType { get; set; }

        public int SeatCapacity { get; set; }

        public string DriverName { get; set; }

        public string DriverMobileNo { get; set; }

        public string VehicleNodalName { get; set; }

        public string NodalMobileNo { get; set; }

        public string District { get; set; }

        public string BlockName { get; set; }

        public string? Gpname { get; set; }

        public string? Remark { get; set; }
    }
}
