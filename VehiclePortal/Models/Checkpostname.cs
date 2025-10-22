using System;
using System.Collections.Generic;

namespace VehiclePortal.Models;

public partial class Checkpostname
{
    public int CheckpostId { get; set; }

    public string? CheckpostName { get; set; }

    public virtual ICollection<Checkpost> Checkposts { get; set; } = new List<Checkpost>();
}
