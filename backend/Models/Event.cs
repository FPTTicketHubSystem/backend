using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Event
{
    public int EventId { get; set; }

    public int AccountId { get; set; }

    public int CategoryId { get; set; }

    public string? EventName { get; set; }

    public string? ThemeImage { get; set; }

    public string? EventDescription { get; set; }

    public string? Location { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? TicketQuantity { get; set; }

    public string? Status { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Discountcode> Discountcodes { get; set; } = new List<Discountcode>();

    public virtual ICollection<Eventimage> Eventimages { get; set; } = new List<Eventimage>();

    public virtual ICollection<Eventrating> Eventratings { get; set; } = new List<Eventrating>();

    public virtual ICollection<Eventstaff> Eventstaffs { get; set; } = new List<Eventstaff>();
}
