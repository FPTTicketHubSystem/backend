using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Orderdetail
{
    public int OrderDetailId { get; set; }

    public int? TicketTypeId { get; set; }

    public int? OrderId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Subtotal { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Tickettype? TicketType { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
