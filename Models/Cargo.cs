using System;
using System.Collections.Generic;

namespace ModaECommerce.Models;

public partial class Cargo
{
    public int CargoId { get; set; }

    public DateTime? CargoTime { get; set; }

    public float? CargoPrice { get; set; }

    public int? CargoLevelId { get; set; }

    public string? CargoNotes { get; set; }

    public float? CargoWeight { get; set; }

    public string? CargoUserName { get; set; }

    public string? CargoUserSurname { get; set; }

    public string? CargoUserPhone { get; set; }

    public string? CargoUserEmail { get; set; }

    public string? CargoAddressCountry { get; set; }

    public string? CargoAddressCity { get; set; }

    public string? CargoAddressDistrict { get; set; }

    public string? CargoAddressStreet { get; set; }

    public string? CargoAddressHouse { get; set; }

    public string? CargoAddressApartment { get; set; }

    public int? CargoUserId { get; set; }

    public virtual ICollection<Baglama> Baglamas { get; set; } = new List<Baglama>();

    public virtual Level? CargoLevel { get; set; }
}
