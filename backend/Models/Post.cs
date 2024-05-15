using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int AccountId { get; set; }

    public string? PostText { get; set; }

    public string? PostFile { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Status { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Postcomment> Postcomments { get; set; } = new List<Postcomment>();

    public virtual ICollection<Postfavorite> Postfavorites { get; set; } = new List<Postfavorite>();

    public virtual ICollection<Postlike> Postlikes { get; set; } = new List<Postlike>();
}
