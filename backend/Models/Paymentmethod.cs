using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Paymentmethod
{
    public int PaymentMethodId { get; set; }

    public string? MethodName { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
