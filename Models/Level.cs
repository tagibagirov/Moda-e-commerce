using System;
using System.Collections.Generic;

namespace ModaECommerce.Models;

public partial class Level
{
    public int LevelId { get; set; }

    public string? LevelName { get; set; }

    public virtual ICollection<Cargo> Cargos { get; set; } = new List<Cargo>();
}
