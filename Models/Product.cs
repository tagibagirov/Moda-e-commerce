using System;
using System.Collections.Generic;

namespace ModaECommerce.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public float? ProductPrice { get; set; }

    public string? ProductPhoto { get; set; }

    public string? ProductAbout { get; set; }

    public DateOnly? ProductYear { get; set; }

    public float? ProductWeight { get; set; }

    public string? ProductCountry { get; set; }

    public string? ProductSize { get; set; }

    public int? ProductRatings { get; set; }

    public float? ProductAverageRating { get; set; }

    public int? ProductCategoryId { get; set; }

    public string? ProductGender { get; set; }

    public int? ProductBrendId { get; set; }

    public string? ProductStatus { get; set; }

    public int? ProductColorId { get; set; }

    public virtual ICollection<Baglama> Baglamas { get; set; } = new List<Baglama>();

    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    public virtual ICollection<Cargo> Cargos { get; set; } = new List<Cargo>();

    public virtual Brand? ProductBrend { get; set; }

    public virtual Category? ProductCategory { get; set; }

    public virtual Color? ProductColor { get; set; }
}
