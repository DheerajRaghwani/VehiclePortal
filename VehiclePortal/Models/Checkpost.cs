using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehiclePortal.Models
{
    public partial class Checkpost
    {
        public Guid Id { get; set; }

        [ForeignKey("Checkpostname")] // tells EF this is the FK
        public int CheckpostId { get; set; }

        public string? VehicleNo { get; set; }

        public int? DistrictId { get; set; }

        public int? BlockId { get; set; }

        public bool? Pass { get; set; }

        public int? TotalPeople { get; set; }

        public DateTime? CurrentDate { get; set; }

        // ✅ Only one navigation property to Checkpostname
        public virtual Checkpostname? Checkpostname { get; set; }

        public virtual Vehicleregistration? VehicleNoNavigation { get; set; }
    }
}
