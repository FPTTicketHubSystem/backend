using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;

public partial class FpttickethubContext : DbContext
{
    public FpttickethubContext()
    {
    }

    public FpttickethubContext(DbContextOptions<FpttickethubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Discountcode> Discountcodes { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Eventimage> Eventimages { get; set; }

    public virtual DbSet<Eventrating> Eventratings { get; set; }

    public virtual DbSet<Eventstaff> Eventstaffs { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Paymentmethod> Paymentmethods { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Postcomment> Postcomments { get; set; }

    public virtual DbSet<Postfavorite> Postfavorites { get; set; }

    public virtual DbSet<Postlike> Postlikes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Tickettype> Tickettypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Integrated Security=SSPI;Encrypt=False;database=fpttickethub");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("ACCOUNT");

            entity.HasIndex(e => e.Email, "UK_Email").IsUnique();

            entity.HasIndex(e => e.Phone, "UK_Phone").IsUnique();

            entity.Property(e => e.Avatar).IsUnicode(false);
            entity.Property(e => e.BirthDay).HasColumnType("date");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Gold).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ACCOUNT_ROLE");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("CATEGORY");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Discountcode>(entity =>
        {
            entity.ToTable("DISCOUNTCODE");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Discountcodes)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DISCOUNTCODE_ACCOUNT");

            entity.HasOne(d => d.Event).WithMany(p => p.Discountcodes)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DISCOUNTCODE_EVENT");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("EVENT");

            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.ThemeImage).IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Events)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EVENT_ACCOUNT");

            entity.HasOne(d => d.Category).WithMany(p => p.Events)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EVENT_CATEGORY");
        });

        modelBuilder.Entity<Eventimage>(entity =>
        {
            entity.ToTable("EVENTIMAGE");

            entity.Property(e => e.ImageUrl).IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Event).WithMany(p => p.Eventimages)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EVENTIMAGE_EVENT");
        });

        modelBuilder.Entity<Eventrating>(entity =>
        {
            entity.ToTable("EVENTRATING");

            entity.Property(e => e.RatingDate).HasColumnType("datetime");
            entity.Property(e => e.Review).HasMaxLength(200);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Account).WithMany(p => p.Eventratings)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EVENTRATING_ACCOUNT");

            entity.HasOne(d => d.Event).WithMany(p => p.Eventratings)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EVENTRATING_EVENT");
        });

        modelBuilder.Entity<Eventstaff>(entity =>
        {
            entity.HasKey(e => new { e.EventId, e.AccountId });

            entity.ToTable("EVENTSTAFF");

            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Eventstaffs)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EVENTSTAFF_ACCOUNT");

            entity.HasOne(d => d.Event).WithMany(p => p.Eventstaffs)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EVENTSTAFF_EVENT");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.ToTable("NEWS");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.Account).WithMany(p => p.News)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NEWS_ACCOUNT");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("ORDER");

            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDER_ACCOUNT");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.ToTable("ORDERDETAIL");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDERDETAIL_ORDER");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("PAYMENT");

            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.DiscountCode).WithMany(p => p.Payments)
                .HasForeignKey(d => d.DiscountCodeId)
                .HasConstraintName("FK_PAYMENT_DISCOUNTCODE");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PAYMENT_ORDER");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PAYMENT_PAYMENTMETHOD");
        });

        modelBuilder.Entity<Paymentmethod>(entity =>
        {
            entity.ToTable("PAYMENTMETHOD");

            entity.Property(e => e.MethodName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("POST");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.PostText).HasMaxLength(1000);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Posts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POST_ACCOUNT");
        });

        modelBuilder.Entity<Postcomment>(entity =>
        {
            entity.ToTable("POSTCOMMENT");

            entity.Property(e => e.CommentDate).HasColumnType("datetime");
            entity.Property(e => e.Content).HasMaxLength(1000);
            entity.Property(e => e.FileComment).IsUnicode(false);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Postcomments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POSTCOMMENT_ACCOUNT");

            entity.HasOne(d => d.Post).WithMany(p => p.Postcomments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POSTCOMMENT_POST");
        });

        modelBuilder.Entity<Postfavorite>(entity =>
        {
            entity.ToTable("POSTFAVORITE");

            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Postfavorites)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POSTFAVORITE_ACCOUNT");

            entity.HasOne(d => d.Post).WithMany(p => p.Postfavorites)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POSTFAVORITE_POST");
        });

        modelBuilder.Entity<Postlike>(entity =>
        {
            entity.ToTable("POSTLIKE");

            entity.Property(e => e.LikeDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Postlikes)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POSTLIKE_ACCOUNT");

            entity.HasOne(d => d.Post).WithMany(p => p.Postlikes)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POSTLIKE_POST");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("ROLE");

            entity.Property(e => e.RoleName).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("TICKET");

            entity.Property(e => e.CheckInDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.TicketCode).IsUnicode(false);

            entity.HasOne(d => d.OrderDetail).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.OrderDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TICKET_ORDERDETAIL");

            entity.HasOne(d => d.TicketType).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TICKET_TICKETTYPE");
        });

        modelBuilder.Entity<Tickettype>(entity =>
        {
            entity.ToTable("TICKETTYPE");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.TypeName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
