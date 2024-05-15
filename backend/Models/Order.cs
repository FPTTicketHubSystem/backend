using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int AccountId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? Total { get; set; }

    public string? Status { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
