using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Tickettype
{
    public int TicketTypeId { get; set; }

    public int EventId { get; set; }

    public string? TypeName { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
