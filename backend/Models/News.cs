using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class News
{
    public int NewsId { get; set; }

    public int AccountId { get; set; }

    public string? Title { get; set; }

    public string? Subtitle { get; set; }

    public string? Content { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Status { get; set; }

    public virtual Account Account { get; set; } = null!;
}
