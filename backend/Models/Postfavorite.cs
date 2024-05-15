using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Postfavorite
{
    public int PostFavoriteId { get; set; }

    public int AccountId { get; set; }

    public int PostId { get; set; }

    public string? Status { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
