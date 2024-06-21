using System;
using System.Collections.Generic;

namespace ModaECommerce.Models;

public partial class Color
{
    public int ColorId { get; set; }

    public string? ColorName { get; set; }

    public string? ColorHexCode { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
