using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public int? RoleId { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? FullName { get; set; }

    public DateTime? BirthDay { get; set; }

    public string? Phone { get; set; }

    public string? Avatar { get; set; }

    public string? Gender { get; set; }

    public string? Status { get; set; }

    public decimal? Gold { get; set; }

    public virtual ICollection<Discountcode> Discountcodes { get; set; } = new List<Discountcode>();

    public virtual ICollection<Eventrating> Eventratings { get; set; } = new List<Eventrating>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Eventstaff> Eventstaffs { get; set; } = new List<Eventstaff>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Postcomment> Postcomments { get; set; } = new List<Postcomment>();

    public virtual ICollection<Postfavorite> Postfavorites { get; set; } = new List<Postfavorite>();

    public virtual ICollection<Postlike> Postlikes { get; set; } = new List<Postlike>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual Role? Role { get; set; }
}
