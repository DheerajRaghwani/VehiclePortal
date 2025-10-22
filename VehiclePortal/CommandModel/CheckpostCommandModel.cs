using VehiclePortal.Models;

namespace VehiclePortal.CommandModel
{
    public class CheckpostCommandModel
    {
        public Guid Id { get; set; }

        public int CheckpostId { get; set; }

        public string? VehicleNo { get; set; }

       

        public bool? Pass { get; set; }

        public int? TotalPeople { get; set; }

        public DateTime? CurrentDate { get; set; }

    }
}
