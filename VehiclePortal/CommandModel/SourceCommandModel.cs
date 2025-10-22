using VehiclePortal.Models;

namespace VehiclePortal.CommandModel
{
    public class SourceCommandModel
    {
        public Guid Id { get; set; }

        public string? VehicleNo { get; set; }

        public int? DistrictId { get; set; }

        public int? BlockId { get; set; }

        public bool? Pass { get; set; }

        public int? TotalPeople { get; set; }

        public DateTime? CurrentDate { get; set; }

   

    }
}
