using System;
using System.Collections.Generic;

namespace ModaECommerce.Models;

public partial class Baglama
{
    public int BaglamaId { get; set; }

    public int? BaglamaCargoId { get; set; }

    public int? BaglamaProductId { get; set; }

    public float? BaglamaWeight { get; set; }

    public int? BaglamaProductQuantity { get; set; }

    public int? BaglamaProductSizeId { get; set; }

    public virtual Cargo? BaglamaCargo { get; set; }

    public virtual Product? BaglamaProduct { get; set; }
}
