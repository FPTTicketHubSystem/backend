using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Eventrating
{
    public int EventRatingId { get; set; }

    public int? EventId { get; set; }

    public int? AccountId { get; set; }

    public int? Rating { get; set; }

    public string? Review { get; set; }

    public DateTime? RatingDate { get; set; }

    public string? Status { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Event? Event { get; set; }
}
