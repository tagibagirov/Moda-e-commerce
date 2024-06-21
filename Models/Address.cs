using System;
using System.Collections.Generic;

namespace ModaECommerce.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public string? AddressCountry { get; set; }

    public string? AddressCity { get; set; }

    public string? AddressDistrict { get; set; }

    public string? AddressStreet { get; set; }

    public string? AddressHouse { get; set; }

    public string? AddressApartment { get; set; }

    public int? AddressUserId { get; set; }

    public virtual User? AddressUser { get; set; }
}
