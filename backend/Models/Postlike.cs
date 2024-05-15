using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Postlike
{
    public int PostLikeId { get; set; }

    public int AccountId { get; set; }

    public int PostId { get; set; }

    public DateTime? LikeDate { get; set; }

    public string? Status { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
