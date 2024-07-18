using System;
using System.Collections.Generic;

namespace ModaECommerce.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserSurname { get; set; }

    public string? UserNickname { get; set; }

    public string? UserEmail { get; set; }

    public string? UserPassword { get; set; }

    public string? UserRole { get; set; }

    public string? UserStatus { get; set; }

    public string? UserPhone { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
