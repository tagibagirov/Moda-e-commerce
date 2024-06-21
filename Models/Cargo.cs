using System;
using System.Collections.Generic;

namespace ModaECommerce.Models;

public partial class Cargo
{
    public int CargoId { get; set; }

    public DateTime? CargoTime { get; set; }

    public float? CargoPrice { get; set; }

    public int? CargoProductId { get; set; }

    public int? CargoUserId { get; set; }

    public int? CargoLevelId { get; set; }

    public virtual ICollection<Baglama> Baglamas { get; set; } = new List<Baglama>();

    public virtual Level? CargoLevel { get; set; }

    public virtual Product? CargoProduct { get; set; }

    public virtual User? CargoUser { get; set; }
}
