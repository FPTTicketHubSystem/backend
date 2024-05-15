using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
