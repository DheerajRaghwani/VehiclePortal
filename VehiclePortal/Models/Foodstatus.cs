using System;
using System.Collections.Generic;

namespace VehiclePortal.Models;

public partial class Foodstatus
{
    public Guid Id { get; set; }

    public string? VehicleNo { get; set; }

    public bool? FoodGiven { get; set; }

    public int? TotalPeople { get; set; }

    public DateTime? CurrentDate { get; set; }

    public byte[]? Photo { get; set; }

    public virtual Vehicleregistration? VehicleNoNavigation { get; set; }
}
