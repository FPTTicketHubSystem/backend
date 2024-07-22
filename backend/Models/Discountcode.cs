using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Discountcode
{
    public int DiscountCodeId { get; set; }

    public int? AccountId { get; set; }

    public int? EventId { get; set; }

    public string? Code { get; set; }

    public int? DiscountAmount { get; set; }

    public int? Quantity { get; set; }

    public string? Status { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Event? Event { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    //public static implicit operator object?(Discountcode v)
    //{
    //    throw new NotImplementedException();
    //}
}
