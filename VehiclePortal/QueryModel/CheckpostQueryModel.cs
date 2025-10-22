namespace VehiclePortal.QueryModel
{
    public class CheckpostQueryModel
    {
        public Guid Id { get; set; }

        public int CheckpostId { get; set; }
        public string CheckpostName { get; set; }

        public string? VehicleNo { get; set; }

        public int? DistrictId { get; set; }

        public string? DistrictName { get; set; } // ✅ converted from DistrictId

        public int? BlockId { get; set; }

        public string? BlockName { get; set; } // ✅ converted from BlockId
        public string? VehicleNodalName { get; set; } // ✅ converted from BlockId
        public string? NodalMobileNo { get; set; } // ✅ converted from BlockId

        public bool? Pass { get; set; }

        public int? TotalPeople { get; set; }

        public DateTime? CurrentDate { get; set; }
    }
}
