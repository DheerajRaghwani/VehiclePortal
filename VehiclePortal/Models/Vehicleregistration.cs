using System;
using System.Collections.Generic;

namespace VehiclePortal.Models;

public partial class Vehicleregistration
{
    public string VehicleNo { get; set; } = null!;

    public string VehicleType { get; set; } = null!;

    public int SeatCapacity { get; set; }

    public string DriverName { get; set; } = null!;

    public string DriverMobileNo { get; set; } = null!;

    public string VehicleNodalName { get; set; } = null!;

    public string NodalMobileNo { get; set; } = null!;

    public int DistrictId { get; set; }

    public int BlockId { get; set; }

    public string? Gpname { get; set; }

    public string? Remark { get; set; }

    public virtual Block Block { get; set; } = null!;

    public virtual Checkpost? Checkpost { get; set; }

    public virtual District District { get; set; } = null!;

    public virtual Source? Source { get; set; }
}
