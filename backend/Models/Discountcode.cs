using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Discountcode
{
    public int DiscountCodeId { get; set; }

    public int AccountId { get; set; }

    public int EventId { get; set; }

    public string? Code { get; set; }

    public int? DiscountAmount { get; set; }

    public int? Quantity { get; set; }

    public string? Status { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
