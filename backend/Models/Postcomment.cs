using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Postcomment
{
    public int PostCommentId { get; set; }

    public int AccountId { get; set; }

    public int PostId { get; set; }

    public string? Content { get; set; }

    public string? FileComment { get; set; }

    public DateTime? CommentDate { get; set; }

    public string? Status { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
