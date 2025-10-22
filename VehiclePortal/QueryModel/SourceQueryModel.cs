using VehiclePortal.Models;

namespace VehiclePortal.QueryModel
{
    public class SourceQueryModel
    {
        public Guid Id { get; set; }

        public string? VehicleNo { get; set; }

        public int? DistrictId { get; set; }

        public int? BlockId { get; set; }

        public bool? Pass { get; set; }

        public int? TotalPeople { get; set; }
        public string? VehicleNodalName { get; set; }
        public string? NodalMobileNo { get; set; }

        public DateTime? CurrentDate { get; set; }
        public string? DistrictName { get; set; }
        public string? Block {  get; set; }

        public virtual Vehicleregistration? VehicleNoNavigation { get; set; }

    }
}
