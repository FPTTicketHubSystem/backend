using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int OrderDetailId { get; set; }

    public int TicketTypeId { get; set; }

    public string? TicketCode { get; set; }

    public string? Status { get; set; }

    public bool? IsCheckedIn { get; set; }

    public DateTime? CheckInDate { get; set; }

    public virtual Orderdetail OrderDetail { get; set; } = null!;

    public virtual Tickettype TicketType { get; set; } = null!;
}
