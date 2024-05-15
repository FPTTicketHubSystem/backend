using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Eventimage
{
    public int EventImageId { get; set; }

    public int EventId { get; set; }

    public string? ImageUrl { get; set; }

    public string? Status { get; set; }

    public virtual Event Event { get; set; } = null!;
}
