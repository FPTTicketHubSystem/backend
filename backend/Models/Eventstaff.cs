using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Eventstaff
{
    public int EventId { get; set; }

    public int AccountId { get; set; }

    public string? Status { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;
}
