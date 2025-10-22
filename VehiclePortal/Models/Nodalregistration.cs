using System;
using System.Collections.Generic;

namespace VehiclePortal.Models;

/// <summary>
/// 		
/// </summary>
public partial class Nodalregistration
{
    public Guid Id { get; set; }

    public string? District { get; set; }

    public string? NodalName { get; set; }

    public string? NodalMobileNo { get; set; }

    public string? AssitantNodalName { get; set; }

    public string? AssitantNodalMobileNo { get; set; }
}
