using System;
using System.Collections.Generic;

namespace VehiclePortal.Models;

public partial class Block
{
    public int BlockId { get; set; }

    public string? Blockname { get; set; }

    public int? DistrictId { get; set; }

    public virtual District? District { get; set; }

    public virtual ICollection<Vehicleregistration> Vehicleregistrations { get; set; } = new List<Vehicleregistration>();
}
