using System;
using System.Collections.Generic;

namespace VehiclePortal.Models;

public partial class District
{
    public int DistrictId { get; set; }

    public string? DistrictName { get; set; }

    public virtual ICollection<Block> Blocks { get; set; } = new List<Block>();

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();

    public virtual ICollection<Vehicleregistration> Vehicleregistrations { get; set; } = new List<Vehicleregistration>();
}
