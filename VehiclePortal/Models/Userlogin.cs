using System;
using System.Collections.Generic;

namespace VehiclePortal.Models;

public partial class Userlogin
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? LoginRole { get; set; }

    public int? DistrictId { get; set; }

    public virtual District? District { get; set; }
}
