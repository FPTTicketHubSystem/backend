using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public int PaymentMethodId { get; set; }

    public int? DiscountCodeId { get; set; }

    public decimal? PaymentAmount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? Status { get; set; }

    public virtual Discountcode? DiscountCode { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Paymentmethod PaymentMethod { get; set; } = null!;
}
