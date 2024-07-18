using System;
using System.Collections.Generic;

namespace ModaECommerce.Models;

public partial class Basket
{
    public int BasketId { get; set; }

    public int? BasketProductId { get; set; }

    public int? BasketUserId { get; set; }

    public int? BasketProductQuantity { get; set; }

    public int? BasketProductSizeId { get; set; }

    public virtual Product? BasketProduct { get; set; }
}
