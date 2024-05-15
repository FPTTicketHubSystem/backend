using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Orderdetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int? TicketId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? Subtotal { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Ticket? Ticket { get; set; }
}
